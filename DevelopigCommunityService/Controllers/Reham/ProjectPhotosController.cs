using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevelopigCommunityService.Context;
using DevelopigCommunityService.Models.Reham;
using DevelopigCommunityService.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace DevelopigCommunityService.Controllers.Reham
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectPhotosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public ProjectPhotosController(DataContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/ProjectPhotos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectPhotos>>> GetProjectPhotos()
        {
            return await _context.ProjectPhotos.ToListAsync();
        }

        // GET: api/ProjectPhotos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectPhotos>> GetProjectPhotos(int id)
        {
            var projectPhotos = await _context.ProjectPhotos.FindAsync(id);

            if (projectPhotos == null)
            {
                return NotFound();
            }

            return projectPhotos;
        }

        // PUT: api/ProjectPhotos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
       // [Authorize]
        public async Task<IActionResult> PutProjectPhotos(int id, ProjectPhotos projectPhotos)
        {

            //String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            //var authUser = _tokenService.GetJWTClams(authHeaders);

            //if (authHeaders == null) return Unauthorized("Only Admin can post OrganizationType");


            if (id != projectPhotos.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectPhotos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectPhotosExists(id))
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

        // POST: api/ProjectPhotos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectPhotos>> PostProjectPhotos(ProjectPhotos projectPhotos)
        {
            _context.ProjectPhotos.Add(projectPhotos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectPhotos", new { id = projectPhotos.Id }, projectPhotos);
        }

        // DELETE: api/ProjectPhotos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectPhotos(int id)
        {
            var projectPhotos = await _context.ProjectPhotos.FindAsync(id);
            if (projectPhotos == null)
            {
                return NotFound();
            }

            _context.ProjectPhotos.Remove(projectPhotos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectPhotosExists(int id)
        {
            return _context.ProjectPhotos.Any(e => e.Id == id);
        }
    }
}
