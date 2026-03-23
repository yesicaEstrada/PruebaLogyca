using Logyca.Data.Models;
using Logyca.Data.Persistence;
using Logyca.Models.CodesDto;
using Logyca.Models.EnterpriseDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaLogyca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CodesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Codes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeDto>>> GetCodes()
        {
            var codes = await _context.Codes
                .Include(c => c.Owner)
                .Select(c => new CodeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    OwnerId = c.OwnerId,
                    OwnerName = c.Owner.Name
                })
                .ToListAsync();

            return Ok(codes);
        }

        // GET: api/Codes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CodeDto>> GetCodesById(int id)
        {
            var code = await _context.Codes
                .Include(c => c.Owner)
                .Select(c => new CodeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    OwnerId = c.OwnerId,
                    OwnerName = c.Owner.Name
                })
                .FirstOrDefaultAsync(e => e.Id == id);

            if (code == null)
            {
                return NotFound();
            }

            return code;
        }

        //obtener los codigos pertenecientews a una empresa
        [HttpGet("Enterprise/{idEnterprise}")]
        public async Task<ActionResult<IEnumerable<CodeDto>>> GetCodeByIdEnterprise(int idEnterprise)
        {
            var codes = await _context.Codes
                .Include(c => c.Owner)
                .Where(c => c.OwnerId == idEnterprise) 
                .Select(c => new CodeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    OwnerId = c.OwnerId,
                    OwnerName = c.Owner.Name
                })
                .ToListAsync();

            if (codes == null || codes.Count == 0)
            {
                return NotFound();
            }

            return Ok(codes);
        }


        // POST: api/Codes
        [HttpPost]
        public async Task<ActionResult<CodeDto>> CreateCode(CodeCreateDto code)
        {
            var codes = new Code
            {
                Name = code.Name,
                Description = code.Description,
                OwnerId = code.OwnerId
            };
            _context.Codes.Add(codes);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCodes), new { id = codes.Id }, codes);
        }

        // PATCH: api/Codes/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCode(int id, [FromBody] UpdateCodeDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest("Datos inválidos");
            }

            var code = await _context.Codes.FindAsync(id);

            if (code == null)
            {
                return NotFound();
            }

            //actualizar solo lo enviado
            if(updateDto.Name != null)
            {
                code.Name = updateDto.Name;
            }

            if (updateDto.Description != null)
            {
                code.Description = updateDto.Description;
            }

            if(updateDto.OwnerId.HasValue)
            {
                var ownerExist = await _context.Enterprises.AnyAsync(e => e.Id == updateDto.OwnerId.Value);

                if(!ownerExist)
                {
                    return BadRequest($"No existe la empresa con Id {updateDto.OwnerId.Value}");
                }

                code.OwnerId = updateDto.OwnerId.Value;
            }

            await _context.SaveChangesAsync();

            return Ok(new CodeDto
            {
                Id = code.Id,
                Name = code.Name,
                Description = code.Description,
                OwnerId = code.OwnerId
            });
        }
    }
}
