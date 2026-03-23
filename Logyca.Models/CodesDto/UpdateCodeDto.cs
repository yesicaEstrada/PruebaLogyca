using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Logyca.Models.CodesDto;

public class UpdateCodeDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? OwnerId { get; set; }
}
