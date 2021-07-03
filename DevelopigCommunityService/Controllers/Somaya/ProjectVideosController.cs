using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DevelopigCommunityService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DevelopigCommunityService.Models.Somaya;
using DevelopigCommunityService.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevelopigCommunityService.Controllers.Somaya
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectVideosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public ProjectVideosController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/<ProjectVideosController>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProjectVideos>>> GetProjectVideos()
        {
            return await _context.ProjectVideos.ToListAsync();
        }

        // GET api/<ProjectVideosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectVideos>> GetProjectsVideos(int id)
        {
            var projectVideos = await _context.ProjectVideos.FindAsync(id);

            if (projectVideos == null) return NotFound();

            return projectVideos;
        }

        // POST api/<ProjectVideosController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProjectVideos>> PostProjectVideo(ProjectVideos projectVideo)
        {
            _context.ProjectVideos.Add(projectVideo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = projectVideo.Id }, projectVideo);
        }

        // PUT api/<ProjectVideosController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProjectVideo(int id, ProjectVideos projectVideo)
        {
            if (id != projectVideo.Id)
                return BadRequest();

            _context.Entry(projectVideo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectVideoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE api/<ProjectVideosController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProjectVideo(int id)
        {
            var projectVideo = await _context.ProjectVideos.FindAsync(id);
            if (projectVideo == null)
                return NotFound();

            _context.ProjectVideos.Remove(projectVideo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectVideoExists(int id)
        {
            return _context.ProjectVideos.Any(e => e.Id == id);
        }
    }
}
