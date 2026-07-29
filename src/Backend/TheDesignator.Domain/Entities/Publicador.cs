namespace TheDesignator.Domain.Entities;

public class Publicador
{
    public string Nome { get; set; } = string.Empty;

    public string Sexo { get; set; } = string.Empty;

    public List<Privilegio>? Privilegios { get; set; }
}
