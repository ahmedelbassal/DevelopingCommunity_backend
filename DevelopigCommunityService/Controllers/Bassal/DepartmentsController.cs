using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevelopigCommunityService.Context;
using DevelopigCommunityService.Models.Aya;
using Microsoft.AspNetCore.Authorization;
using DevelopigCommunityService.Interfaces;
using DevelopigCommunityService.DTOs.Bassal;

namespace DevelopigCommunityService.Controllers.Bassal
{
    public class DepartmentsController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public DepartmentsController(DataContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {

            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            var authUser=_tokenService.GetJWTClams(authHeaders);

            if (authUser.IsAdmin == false) return Unauthorized("Only Admin can Update Department");

            if (id != department.Id)
            {
                return BadRequest();
            }

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await DepartmentExists(id))
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

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Department>> PostDepartment(DepartmentDTO newDepartment)
        {


            //String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            //var authUser = _tokenService.GetJWTClams(authHeaders);

            //if (authUser.IsAdmin == false) return Unauthorized("Only Admin can add new Departments");

            
           if(await DepartmentExists(newDepartment.Name.ToLower()))
            {
                return Conflict("Department with this name already exists");
            }

            Department newDept = new Department
            {
                Name = newDepartment.Name.ToLower(),
                Description = newDepartment.Description,
                IsActive = true
            };

            _context.Departments.Add(newDept);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = newDept.Id }, newDept);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {

            //String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            //var authUser = _tokenService.GetJWTClams(authHeaders);

            //if (authUser.IsAdmin == false) return Unauthorized("Only Admin can add new Departments");



            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            await _context.Individuals.Where(ww => ww.DepartmentId == id).ForEachAsync(ww => ww.DepartmentId = null);
            await _context.Students.Where(ww => ww.DepartmentId == id).ForEachAsync(ww => ww.DepartmentId = null);
            await _context.Instructors.Where(ww => ww.DepartmentId == id).ForEachAsync(ww => ww.DepartmentId = null);

            department.IsActive = false;

            _context.Entry(department).State = EntityState.Modified;

            //_context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> DepartmentExists(int id)
        {
            return await _context.Departments.AnyAsync(e => e.Id == id);
        } 
        
        private async Task<bool> DepartmentExists(String DeptName)
        {
            return await _context.Departments.AnyAsync(e => e.Name == DeptName);
        }
    }
}
