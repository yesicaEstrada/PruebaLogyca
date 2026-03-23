using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Logyca.Models.CodesDto;
public class CodeDto
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
    public string? Description { get; set; }

    public int OwnerId { get; set; }

    public string OwnerName { get; set; }
}
