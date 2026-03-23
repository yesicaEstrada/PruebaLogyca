using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Logyca.Data.Models;

[Table("Enterprise")]
public class Enterprise
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    public long? Nit { get; set; }

    public long Gln { get; set; }

    //una empresa tiene muchos codigos
    //lista del modelo codes
    public ICollection<Code>? Codes { get; set; }
}
