using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Logyca.Models.EnterpriseDto;

public class EnterpriseCreateDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; }

    public long? Nit { get; set; }

    public long Gln { get; set; }
}
