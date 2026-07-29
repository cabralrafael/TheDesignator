using System;
using System.Collections.Generic;
using System.Text;

namespace TheDesignator.Domain.Entities;

public class Privilegio
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public bool PermiteAlterarDesignacao { get; set; }
}
