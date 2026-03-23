using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Logyca.Models.CodesDto;

public class CodeCreateDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public int OwnerId { get; set; }
}
