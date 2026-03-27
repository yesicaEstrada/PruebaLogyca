using Logyca.Data.Models;
using Logyca.Data.Persistence;
using Logyca.Models.CodesDto;
using Logyca.Models.EnterpriseDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaLogyca.Services;
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
        private readonly CodeService _codeService;

        private readonly AppDbContext _context;

        public CodesController(AppDbContext context, CodeService codeService)
        {
            _context = context;
            _codeService = codeService;
        }

        // GET: api/Codes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeDto>>> GetCodes()
        {
            var codes = await _codeService.GetCodes();

            return Ok(codes);
        }

        // GET: api/Codes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CodeDto>> GetCodesById(int id)
        {
            var code = await _codeService.GetCodeById(id);
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
            var codes = await _codeService.GetCodeByIdEnterpriseAsync(idEnterprise);

            if (codes != null)
            {
                return Ok(codes);
            }

            return NotFound();
        }


        // POST: api/Codes
        [HttpPost]
        public async Task<ActionResult<CodeDto>> CreateCode(CodeCreateDto code)
        {
            var codes = await _codeService.CreateCodeAsync(code);

            return codes;
        }

        // PATCH: api/Codes/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCode(int id, [FromBody] UpdateCodeDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest("Datos inválidos");
            }

            var code = await _codeService.UpdateCodeAsync(id, updateDto);

            return Ok(code);
        }
    }
}
