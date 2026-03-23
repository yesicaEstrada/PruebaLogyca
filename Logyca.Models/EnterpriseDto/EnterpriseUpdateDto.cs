using System;
using System.Collections.Generic;
using System.Text;

namespace Logyca.Models.EnterpriseDto;

public class EnterpriseUpdateDto
{
    public string? Name { get; set; }

    public long? Nit { get; set; }

    public long? Gln { get; set; }
}
