using Logyca.Data.Models;
using Logyca.Data.Persistence;
using Logyca.Models.CodesDto;
using Logyca.Models.EnterpriseDto;
using Microsoft.EntityFrameworkCore;

namespace PruebaLogyca.Services;

public class EnterpriseService
{
    public readonly AppDbContext _context;

    public EnterpriseService(AppDbContext context)
    {
         _context = context;
    }

    public async Task<List<EnterpriseDto>> GetEnterprises()
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
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    }).ToList()
                }).ToListAsync();
    }

    public async Task<EnterpriseDto?> GetEnterpriseByNitAsync(long nit)
    {
        return await _context.Enterprises
            .AsNoTracking()
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
            })
            .FirstOrDefaultAsync();
    }


    public async Task<IEnumerable<EnterpriseDto>> GetEnterpriseByCode(int codeId)
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
        return result != null ? new List<EnterpriseDto> { result } : new List<EnterpriseDto>();
    }

    public async Task<IEnumerable<EnterpriseDto?>> GetEnterpriseByIdAsync(int id)
    {
        var resul =  await _context.Enterprises
            .AsNoTracking()
            .Where(e => e.Id == id)
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
            })
            .FirstOrDefaultAsync();

        return resul != null ? new List<EnterpriseDto?> { resul } : new List<EnterpriseDto?>();
    }

    public async Task<EnterpriseDto> EnterpriseUpdateAsync(int id, EnterpriseUpdateDto enterprisedto)
    {
        var enterprise = await _context.Enterprises.FindAsync(id);
        if (enterprise == null)
        {
            throw new KeyNotFoundException($"Enterprise with ID {id} not found.");
        }

        if (enterprisedto.Name != null)
        {
            enterprise.Name = enterprisedto.Name;
        }

        if (enterprisedto.Nit != null)
        {
            enterprise.Nit = enterprisedto.Nit;
        }

        if (enterprisedto.Gln != null)
        {
            enterprise.Gln = enterprisedto.Gln.Value;
        }

        _context.Enterprises.Update(enterprise);

        await _context.SaveChangesAsync();

        return new EnterpriseDto
        {
            Id = enterprise.Id,
            Name = enterprise.Name,
            Nit = enterprise.Nit,
            Gln = enterprise.Gln
        };
    }

    public async Task<EnterpriseDto> CreateEnterpriseAsync(EnterpriseCreateDto enterpriseCreateDto)
    {
        var enterprise = new Enterprise
        {
            Name = enterpriseCreateDto.Name,
            Nit = enterpriseCreateDto.Nit,
            Gln = enterpriseCreateDto.Gln
        };
        _context.Enterprises.Add(enterprise);
        await _context.SaveChangesAsync();
        return new EnterpriseDto
        {
            Id = enterprise.Id,
            Name = enterprise.Name,
            Nit = enterprise.Nit,
            Gln = enterprise.Gln
        };
    }
}
