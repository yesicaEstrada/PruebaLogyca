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
using PruebaLogyca.Services;

namespace PruebaLogyca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterprisesController : ControllerBase
    {
        private readonly EnterpriseService _enterpriseService;

        public EnterprisesController(EnterpriseService service)
        {
            _enterpriseService = service;
        }

        // GET: api/Enterprises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnterpriseDto>>> GetEnterprises()
        {
            var enterprises = await _enterpriseService.GetEnterprises();

            return Ok(enterprises);
        }

        //GET: api/ByNit/12544
        [HttpGet("ByNit/{nit}")]
        public async Task<ActionResult<IEnumerable<EnterpriseDto>>> GetEnterpriseByNit(long nit)
        {
            var enterprise  = await _enterpriseService.GetEnterpriseByNitAsync(nit);

            if (enterprise == null)
            {

                return NotFound();
            }
            return Ok(enterprise);
        }

        [HttpGet("enterprise/byCodigo/{codeId}")]
        public async Task<ActionResult<IEnumerable<EnterpriseDto>>> GetEnterpriseByCode(int codeId)
        {
            var result = await _enterpriseService.GetEnterpriseByCode(codeId);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }


        // GET: api/Enterprises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enterprise>> GetEnterprise(int id)
        {
            var enterprise = await _enterpriseService.GetEnterpriseByIdAsync(id);

            if (enterprise == null)
            {
                return NotFound();
            }

            return Ok(enterprise);
        }

        // PUT: api/Enterprises/5
        [HttpPatch("actualizar/{id}")]
        public async Task<ActionResult<EnterpriseDto>> EditEnterprise(int id, [FromBody] EnterpriseUpdateDto enterprisedto)
        {
            if (enterprisedto == null)
            {
                return BadRequest("Datos inválidos");
            }

            var enterprise = await _enterpriseService.EnterpriseUpdateAsync(id, enterprisedto);

            if (enterprise == null) 
            {
                return BadRequest(enterprise);
            }

            return Ok(enterprise);
        }

        // POST: api/Enterprises
        [HttpPost]
        public async Task<ActionResult<EnterpriseDto>> SaveEnterprise(EnterpriseCreateDto enterprise)
        {
            var enterprises = await _enterpriseService.CreateEnterpriseAsync(enterprise);

            return Ok(enterprises);
        }
    }
}
