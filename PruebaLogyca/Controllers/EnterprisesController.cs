using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Logyca.Data.Persistence;
using Logyca.Models;
using Logyca.Data.Models;
using Logyca.Models.EnterpriseDto;
using Logyca.Models.CodesDto;

namespace PruebaLogyca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterprisesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnterprisesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Enterprises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnterpriseDto>>> GetEnterprises()
        {
            return await _context.Enterprises
                .Include(e => e.Codes)
                .Select(e => new EnterpriseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Nit = e.Nit,
                    Gln = e.Gln,
                    Codes = e.Codes.Select(c => new CodeDto
                    {
                        Id =c.Id,
                        Name=c.Name,
                        Description = c.Description
                    }).ToList()
                }).ToListAsync();
        }

        //GET: api/ByNit/12544
        [HttpGet("ByNit/{nit}")]
        public async Task<ActionResult<IEnumerable<EnterpriseDto>>> GetEnterpriseByNit(long nit)
        {
            var enterprise = await _context.Enterprises
                .Include(e => e.Codes)
                .Where(e => e.Nit == nit)
                .Select(e => new EnterpriseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Nit = e.Nit,
                    Gln = e.Gln,
                    Codes = e.Codes.Select(c => new CodeDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    }).ToList()
                }).FirstOrDefaultAsync();

            if(enterprise == null) {

                return NotFound();
            }
            return Ok(enterprise);
        }

        [HttpGet("enterprise/byCodigo/{codeId}")]
        public async Task<ActionResult<IEnumerable<EnterpriseDto>>> GetEnterpriseByCode(int codeId)
        {
            var result = await _context.Codes
                .Where(c => c.Id == codeId)
                .Select(c => new EnterpriseDto
                {
                    Id = c.OwnerId,
                    Name = c.Owner.Name,
                    Nit = c.Owner.Nit,
                    Gln = c.Owner.Gln,
                    Codes = new List<CodeDto>
                    {
                        new CodeDto{
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description
                        }
                    }
                }).FirstOrDefaultAsync();

            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }


        // GET: api/Enterprises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enterprise>> GetEnterprise(int id)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);

            if (enterprise == null)
            {
                return NotFound();
            }

            return enterprise;
        }

        // PUT: api/Enterprises/5
        [HttpPatch("actualizar/{id}")]
        public async Task<IActionResult> EditEnterprise(int id, [FromBody] EnterpriseUpdateDto enterprisedto)
        {
            if (enterprisedto == null)
            {
                return BadRequest("Datos inválidos");
            }

            var enterprise = await _context.Enterprises.FindAsync(id);

            if (enterprise == null) 
            {
                return NotFound();
            }

            if(enterprisedto.Name != null)
            {
                enterprise.Name = enterprisedto.Name;
            }

            if(enterprisedto.Nit != null)
            {
                enterprise.Nit = enterprisedto.Nit;
            }

            if(enterprisedto.Gln != null)
            {
                enterprise.Gln = enterprisedto.Gln.Value;
            }

            await _context.SaveChangesAsync();

            return Ok(new EnterpriseDto
            {
                Id = enterprise.Id,
                Name = enterprise.Name,
                Nit = enterprisedto.Nit,
                Gln = enterprise.Gln
            });
        }

        // POST: api/Enterprises
        [HttpPost]
        public async Task<ActionResult<EnterpriseDto>> SaveEnterprise(EnterpriseDto enterprise)
        {
            var enterprises = new Enterprise
            {
                Name = enterprise.Name,
                Nit = enterprise.Nit,
                Gln = enterprise.Gln
            };
            _context.Enterprises.Add(enterprises);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnterprise), new { id =  enterprises.Id }, enterprises);
        }
    }
}
