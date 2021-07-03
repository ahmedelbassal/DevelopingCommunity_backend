using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevelopigCommunityService.Context;
using DevelopigCommunityService.Models.Maher;


// ADDED NAMESPACES
using DevelopigCommunityService.Interfaces;
using Microsoft.AspNetCore.Authorization;
//

namespace DevelopigCommunityService.Controllers.Maher
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectFilesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public ProjectFilesController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/ProjectFiles
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<ProjectFiles>>> GetProjectFiles()
        {
            return await _context.ProjectFiles.ToListAsync();
        }

        // GET: api/ProjectFiles/5
        [HttpGet("{id}")]        
        public async Task<ActionResult<ProjectFiles>> GetProjectFiles(int id)
        {
            var projectFiles = await _context.ProjectFiles.FindAsync(id);

            if (projectFiles == null)
            {
                return NotFound();
            }

            return projectFiles;
        }

        // PUT: api/ProjectFiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProjectFiles(int id, ProjectFiles projectFiles)
        {
            if (id != projectFiles.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectFiles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectFilesExists(id))
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

        // POST: api/ProjectFiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProjectFiles>> PostProjectFiles(ProjectFiles projectFiles)
        {
            _context.ProjectFiles.Add(projectFiles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectFiles", new { id = projectFiles.Id }, projectFiles);
        }

        // DELETE: api/ProjectFiles/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProjectFiles(int id)
        {
            var projectFiles = await _context.ProjectFiles.FindAsync(id);
            if (projectFiles == null)
            {
                return NotFound();
            }

            _context.ProjectFiles.Remove(projectFiles);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectFilesExists(int id)
        {
            return _context.ProjectFiles.Any(e => e.Id == id);
        }
    }
}
