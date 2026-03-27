using Logyca.Data.Models;
using Logyca.Data.Persistence;
using Logyca.Models.CodesDto;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace PruebaLogyca.Services;

public class CodeService
{
    private readonly AppDbContext _context;

    public CodeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CodeDto>> GetCodes()
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

        return codes;
    }

    public async Task<CodeDto?> GetCodeById(int id)
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
        return code;

    }

    public async Task<IEnumerable<CodeDto>> GetCodeByIdEnterpriseAsync(int id)
    {
        var codes = await _context.Codes
                .Include(c => c.Owner)
                .Where(c => c.OwnerId == id)
                .Select(c => new CodeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    OwnerId = c.OwnerId,
                    OwnerName = c.Owner.Name
                })
                .ToListAsync();
        return codes;
    }

    public async Task<CodeDto> CreateCodeAsync(CodeCreateDto code)
    {
        var codes = new Code
        {
            Name = code.Name,
            Description = code.Description,
            OwnerId = code.OwnerId
        };
        _context.Codes.Add(codes);
        await _context.SaveChangesAsync();
        return new CodeDto
        { 
            Id = codes.Id,
            Name = codes.Name,
            Description = codes.Description,
            OwnerId = codes.OwnerId,
            OwnerName = (await _context.Enterprises.FindAsync(codes.OwnerId))?.Name
            
        };
    }

    public async Task<CodeDto> UpdateCodeAsync(int id, UpdateCodeDto updateDto)
    {
        var code = await _context.Codes.FindAsync(id);
        if (code == null)
        {
            throw new KeyNotFoundException($"Enterprise with ID {id} not found.");
        }
        if (updateDto.Name != null)
        {
            code.Name = updateDto.Name;
        }
        if (updateDto.Description != null)
        {
            code.Description = updateDto.Description;
        }
        if (updateDto.OwnerId.HasValue)
        {
            var ownerExist = await _context.Enterprises.AnyAsync(e => e.Id == updateDto.OwnerId.Value);
            if (!ownerExist)
            {
                throw new KeyNotFoundException($"Enterprise with ID {id} not found."); // No se encontró la empresa
            }
            code.OwnerId = updateDto.OwnerId.Value;
        }
        await _context.SaveChangesAsync();
        return new CodeDto
        {
            Id = code.Id,
            Name = code.Name,
            Description = code.Description,
            OwnerId = code.OwnerId
        };
    }
}