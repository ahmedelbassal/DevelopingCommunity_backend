using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevelopigCommunityService.Context;
using DevelopigCommunityService.Models.Aya;
using DevelopigCommunityService.Interfaces;

namespace DevelopigCommunityService.Controllers.Bassal
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationTypesController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;
        public OrganizationTypesController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/OrganizationTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationType>>> GetOrganizationTypes()
        {
            return await _context.OrganizationTypes.ToListAsync();
        }

        // GET: api/OrganizationTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationType>> GetOrganizationType(int id)
        {
            var organizationType = await _context.OrganizationTypes.FindAsync(id);

            if (organizationType == null)
            {
                return NotFound();
            }

            return organizationType;
        }

        // PUT: api/OrganizationTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganizationType(int id, OrganizationType organizationType)
        {
            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authHeaders == null) return Unauthorized("Only Admin can post OrganizationType");


            if (authUser.IsAdmin == false) return Unauthorized("Only Admin can Update OrganizationType");




            if (id != organizationType.Id)
            {
                return BadRequest();
            }

            _context.Entry(organizationType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrganizationTypeExists(id))
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

        // POST: api/OrganizationTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrganizationType>> PostOrganizationType(OrganizationType organizationType)
        {
            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authHeaders == null) return Unauthorized("Only Admin can post OrganizationType");


            if (authUser.IsAdmin == false) return Unauthorized("Only Admin can post OrganizationType");


            await _context.OrganizationTypes.AddAsync(organizationType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrganizationType", new { id = organizationType.Id }, organizationType);
        }

        // DELETE: api/OrganizationTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganizationType(int id)
        {

            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authHeaders == null) return Unauthorized("Only Admin can post OrganizationType");
            if (authUser.IsAdmin == false) return Unauthorized("Only Admin can post OrganizationType");


            var organizationType = await _context.OrganizationTypes.FindAsync(id);
            if (organizationType == null)
            {
                return NotFound();
            }

            _context.OrganizationTypes.Remove(organizationType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> OrganizationTypeExists(int id)
        {
            return await _context.OrganizationTypes.AnyAsync(e => e.Id == id);
        }
    }
}
