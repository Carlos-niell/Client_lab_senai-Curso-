namespace ClientLab.Models;

public abstract class Pessoa
{
    public string Nome { get; private set; }
    public string Endereco { get; private set; }
    public string Telefone { get; private set; }

    protected Pessoa(string nome, string endereco, string telefone)
    {
        Nome = ExigirTexto(nome, nameof(nome));
        Endereco = ExigirTexto(endereco, nameof(endereco));
        Telefone = ExigirTexto(telefone, nameof(telefone));
    }

    protected static string ExigirTexto(string valor, string nomeParametro)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException("O campo deve ser informado.", nomeParametro);
        }

        return valor.Trim();
    }
}