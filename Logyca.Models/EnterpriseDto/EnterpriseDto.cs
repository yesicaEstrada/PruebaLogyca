using Logyca.Models.CodesDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Logyca.Models.EnterpriseDto;
public class EnterpriseDto
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    public long? Nit { get; set; }

    public long Gln { get; set; }

    public List<CodeDto> Codes { get; set; }
    
}
