using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevelopigCommunityService.Context;
using DevelopigCommunityService.Models.Ebtisam;
using DevelopigCommunityService.DTOs.Ebtisam;
using System.Security.Cryptography;
using System.Text;
using DevelopigCommunityService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using DevelopigCommunityService.DTOs.AbstractClasses;
using DevelopigCommunityService.DTOs.Bassal;

namespace DevelopigCommunityService.Controllers.Ebtisam
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        
        public StudentsController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/Students
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.Where(ww=>ww.IsActive==false).Include(ww=>ww.Department).ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            if (student.IsActive == false) return NotFound("User no longer exists");


            return student;
        }


        // get details from token
        [HttpGet("myDetails")]
        public async Task<ActionResult<AppUserEditDetailsDTO>> GetDetailsByToken()
        {
            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            if (authHeaders == null) return Unauthorized("Owner of account can only modify his password");

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authUser.Type != "Student") return Unauthorized("Only owner of this account can modify his data. Login first");

            var student = await _context.Students.FindAsync(authUser.Id);

            if (student == null)
            {
                return NotFound();
            }

            if (student.IsActive == false) return NotFound("User no longer exists");

            AppUserEditDetailsDTO individualEditData = new AppUserEditDetailsDTO
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age,
                Phone = student.Phone,
                Email = student.Email,
                DeptId = student.DepartmentId

            };



            return individualEditData;
        }


        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("password/{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentChangePasswordDTO studenindividualNewData)
        {
            if (id != studenindividualNewData.Id)
            {
                return BadRequest();
            }

            if (studenindividualNewData.NewPassword != studenindividualNewData.ConfNewPassword) return BadRequest("password and confPassword dont match");

            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            if (authHeaders == null) return Unauthorized("Owner of account can only modify his password");

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authUser.Id != id) return Unauthorized("Only owner of this account can modify his data. Login first");

            var EditStudent = await _context.Students.FindAsync(id);

            using var hmac = new HMACSHA512();

            EditStudent.PasswordHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(studenindividualNewData.NewPassword));
            EditStudent.PasswordSalt = hmac.Key;


            _context.Entry(EditStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("details/{id}")]
        public async Task<IActionResult> PutStudent(int id, AppUserEditDetailsDTO studentlNewData)
        {
            if (id != studentlNewData.Id)
            {
                return BadRequest();
            }

            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authUser.Id != id) return Unauthorized("Only owner of this account can modify his data. Login first");

            var EditStudent = await _context.Students.FindAsync(id);

            EditStudent.FirstName = studentlNewData.FirstName;
            EditStudent.LastName = studentlNewData.LastName;
            EditStudent.Phone = studentlNewData.Phone;
            EditStudent.Age = studentlNewData.Age;


            _context.Entry(EditStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }





        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Register")]
        public async Task<ActionResult<Student>> Register(StudentRegisterDTO StudentRegister)

        {
            if (await StudentExists(StudentRegister.UserName.ToLower())) return BadRequest("Username already exists");
            


            using var hmac = new HMACSHA512();
            var newstudent = new Student
            {

                UserName = StudentRegister.UserName.ToLower(),
                FirstName = StudentRegister.FirstName,
                
                Age = StudentRegister.Age,
                Email = StudentRegister.Email,
                Phone = StudentRegister.Phone,
                PasswordHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(StudentRegister.Password)),
                PasswordSalt = hmac.Key
            };

            object p = await _context.Students.AddAsync(newstudent);

            int v = await _context.SaveChangesAsync();

            return Ok("Created successfully");
            //_context.Students.Add(student);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<StudentsDTO>> Login(StudentloginDTO studentlogin)
        {
            var user = await _context.Students
               .SingleOrDefaultAsync(ww => ww.UserName == studentlogin.UserName.ToLower());

            if (user == null) return Unauthorized("Username or password is invalid");

            if (user.IsActive == false) return NotFound("User no longer exists");


            using var hmac = new HMACSHA512(user.GetPasswordSalt());
            var ComputeHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(studentlogin.Password));

            byte[] passwordHash = user.GetPasswordHash();
                

            for (int i = 0; i < ComputeHash.Length; i++)
            {
                if (ComputeHash[i] != passwordHash[i]) return Unauthorized("Invalid Password");
            }

            return new StudentsDTO
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };


        }

       



        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.IsActive = false;

            _context.Entry(student).State = EntityState.Modified;

            //_context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private async Task<bool> StudentExists(string UserNameRegistered)
        {
            return await _context.Students.AnyAsync(e => e.UserName == UserNameRegistered);
        }
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }


    }
}
