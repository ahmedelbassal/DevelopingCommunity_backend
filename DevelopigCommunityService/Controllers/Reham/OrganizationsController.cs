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
    public class OrganizationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public OrganizationsController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/Organizations
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizations()
        {
            return await _context.Organizations.Include(ww=>ww.OrganizationType).ToListAsync();
        }

        // GET: api/Organizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(int id)
        {
            var organization = await _context.Organizations.Include(ww => ww.OrganizationType).FirstOrDefaultAsync(ww=>ww.Id==id);

            if (organization == null)
            {
                return NotFound();
            }

            return organization;
        }

        // PUT: api/Organizations/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutOrganization(int id, Organization organization)
        {
            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authUser.IsAdmin == false) return Unauthorized("Only Admin can Update Organization");

            if (id != organization.Id)
            {
                return BadRequest();
            }

            _context.Entry(organization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationExists(id))
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

        // POST: api/Organizations
        
        [HttpPost]
       [Authorize]
        public async Task<ActionResult<Organization>> PostOrganization(Organization organization)
        {
            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            var authUser = _tokenService.GetJWTClams(authHeaders);
            if (authHeaders == null) return Unauthorized("Only Admin can post OrganizationType");

            if (authUser.IsAdmin == false) return Unauthorized("Only Admin can Update Organization");

            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrganization", new { id = organization.Id }, organization);
        }

        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            var authUser = _tokenService.GetJWTClams(authHeaders);

            if (authUser.IsAdmin == false) return Unauthorized("Only Admin can Update Organization");

            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }

            _context.Organizations.Remove(organization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrganizationExists(int id)
        {
            return _context.Organizations.Any(e => e.Id == id);
        }
    }
}
