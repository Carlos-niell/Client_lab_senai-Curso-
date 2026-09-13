namespace ClientLab.Models;

public abstract class Pessoa
{
    public string Nome { get; private set; }
    public Endereco Endereco { get; private set; }
    public string Telefone { get; private set; }
    public decimal Valor { get; protected set; }
    public decimal ValorImposto { get; protected set; }
    public decimal Total { get; protected set; }

    protected Pessoa(string nome, Endereco endereco, string telefone)
    {
        Nome = ExigirTexto(nome, nameof(nome));
        Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco), "O endereço deve ser informado.");
        Telefone = ExigirTexto(telefone, nameof(telefone));
    }

    public abstract void PagarImposto(decimal valor);

    protected static string ExigirTexto(string valor, string nomeParametro)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException("O campo deve ser informado.", nomeParametro);
        }

        return valor.Trim();
    }
}