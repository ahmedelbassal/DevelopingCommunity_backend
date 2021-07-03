using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevelopigCommunityService.Context;
using DevelopigCommunityService.Models.Bassal;
using DevelopigCommunityService.DTOs.Bassal;
using System.Security.Cryptography;
using System.Text;
using DevelopigCommunityService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using DevelopigCommunityService.DTOs.AbstractClasses;
using DevelopigCommunityService.Models.Aya;

namespace DevelopigCommunityService.Controllers.Bassal
{
    public class IndividualsController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public IndividualsController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/Individuals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Individual>>> GetIndividuals()
        {

            return await _context.Individuals.Where(ww => ww.IsActive == false).Include(ss=>ss.Department).ToListAsync();

            //return await _context.Individuals.Include(WW=>WW.Department).ToListAsync();
        }

        // GET: api/Individuals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Individual>> GetIndividual(int id)
        {
            var individual = await _context.Individuals.FindAsync(id);

            if (individual == null)
            {
                return NotFound();
            }

           if (individual.IsActive == false) return NotFound("User no longer exists");

            //return new IndividualReturnDataDTOs
            //{
            //    UserName = individual.UserName,
            //    Age = individual.Age,
            //    Email = individual.Email,
            //    FirstName = individual.FirstName,
            //    Id = individual.Id,
            //    LastName = individual.LastName,
            //    Phone = individual.Phone,

            //};
            return individual;
        }


        // get details from token
        [HttpGet("myDetails")]
        public async Task<ActionResult<AppUserEditDetailsDTO>> GetDetailsByToken()
        {
            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            if (authHeaders == null) return Unauthorized("Owner of account can only modify his password");

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if ( authUser.Type != "Individual") return Unauthorized("Only owner of this account can modify his data. Login first");

            var individual = await _context.Individuals.FindAsync(authUser.Id);

            if (individual == null)
            {
                return NotFound();
            }

            if (individual.IsActive == false) return NotFound("User no longer exists");

            AppUserEditDetailsDTO individualEditData = new AppUserEditDetailsDTO
            {
                Id=individual.Id,
                FirstName=individual.FirstName,
                LastName=individual.LastName,
                Age=individual.Age,
                Phone=individual.Phone,
                Email=individual.Email,
                DeptId=individual.DepartmentId
                
            };



            return individualEditData;
        }





        // PUT: api/Individuals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("password/{id}")]
        public async Task<IActionResult> PutIndividualPassword(int id, IndividualChangePasswordDTO individualNewData)
        {
            if (id != individualNewData.Id)
            {
                return BadRequest();
            }

            if (individualNewData.NewPassword != individualNewData.ConfNewPassword) return BadRequest("password and confPassword dont match");

            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            if (authHeaders == null) return Unauthorized("Owner of account can only modify his password");

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authUser.Id != id &&authUser.Type!= "Individual") return Unauthorized("Only owner of this account can modify his data. Login first");



            var EditIndividual = await _context.Individuals.FindAsync(id);

            using var hmac = new HMACSHA512();

            EditIndividual.PasswordHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(individualNewData.NewPassword));
            EditIndividual.PasswordSalt = hmac.Key;


            _context.Entry(EditIndividual).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IndividualExists(id))
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



        // PUT: api/Individuals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("details/{id}")]
        public async Task<IActionResult> PutIndividualDetails(int id, AppUserEditDetailsDTO individualNewData)
        {

            if (id != individualNewData.Id)
            {
                return BadRequest();
            }



            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authUser.Id != id && authUser.Type != "Individual") return Unauthorized("Only owner of this account can modify his data. Login first");


            var EditIndividual = await _context.Individuals.FindAsync(id);

            EditIndividual.FirstName = individualNewData.FirstName;
            EditIndividual.LastName = individualNewData.LastName;
            EditIndividual.Phone = individualNewData.Phone;
            EditIndividual.Age = individualNewData.Age;



            _context.Entry(EditIndividual).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IndividualExists(id))
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






        // POST: api/Individuals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Register")]
        public async Task<ActionResult<Individual>> Register(IndividualRegisterDTOs IndividualRegister)
        {

            if (IndividualRegister.Password != IndividualRegister.ConfPassword) return BadRequest("Password and ConfPassword don't match");

            if (await IndividualExists(IndividualRegister.UserName.ToLower())) return BadRequest("Username already exists");

            using var hmac = new HMACSHA512();

            var newIndividual = new Individual
            {
                UserName = IndividualRegister.UserName.ToLower(),
                FirstName = IndividualRegister.FirstName,
                LastName = IndividualRegister.LastName,
                Age = IndividualRegister.Age,
                Email = IndividualRegister.Email,
                Phone = IndividualRegister.Phone,
                DepartmentId = IndividualRegister.DepartId,
                PasswordHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(IndividualRegister.Password)),
                PasswordSalt = hmac.Key,
                IsActive=true
            };

            await _context.Individuals.AddAsync(newIndividual);

            await _context.SaveChangesAsync();

            return Ok("Created successfully");

            //_context.Individuals.Add(individual);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetIndividual", new { id = individual.Id }, individual);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<IndividualDTOs>> Login(IndividualLoginDTO IndividualLogin)
        {

            var user = await _context.Individuals
                .SingleOrDefaultAsync(ww => ww.UserName == IndividualLogin.UserName.ToLower());

            if (user == null) return Unauthorized("Username or password is invalid");

            if (user.IsActive == false) return NotFound("User no longer exists");

            using var hmac = new HMACSHA512(user.GetPasswordSalt());

            var ComputeHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(IndividualLogin.Password));

            byte[] passwordHash = user.GetPasswordHash();

            for (int i = 0; i < ComputeHash.Length; i++)
            {
                if (ComputeHash[i] != passwordHash[i]) return Unauthorized("Invalid Password");
            }

            return new IndividualDTOs
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }


        // DELETE: api/Individuals/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteIndividual(int id)
        {

            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authUser.Id != id && authUser.Type != "Individual") {

                if (authUser.IsAdmin == false)
                {
                    return Unauthorized("Only owner of this account can modify his data. Login first");
                }
                    
             };



            var individual = await _context.Individuals.FindAsync(id);
            if (individual == null)
            {
                return NotFound();
            }

            individual.IsActive = false;

            _context.Entry(individual).State = EntityState.Modified;
            //_context.Individuals.Remove(individual);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        private async Task<bool> IndividualExists(int id)
        {
            return await _context.Individuals.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> IndividualExists(String UserNameRegistered)
        {

            return await _context.Individuals.AnyAsync(e => e.UserName == UserNameRegistered);
        }

    }
}
