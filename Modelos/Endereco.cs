using System.Text.RegularExpressions;

namespace ClientLab.Models;

public sealed class Endereco
{
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Cep { get; private set; }

    public Endereco(
        string logradouro,
        string numero,
        string bairro,
        string cidade,
        string estado,
        string cep,
        string complemento = "")
    {
        Logradouro = ExigirTexto(logradouro, nameof(logradouro));
        Numero = ExigirTexto(numero, nameof(numero));
        Bairro = ExigirTexto(bairro, nameof(bairro));
        Cidade = ExigirTexto(cidade, nameof(cidade));
        Estado = ExigirTexto(estado, nameof(estado));
        Cep = ValidarCep(cep);
        Complemento = complemento?.Trim() ?? string.Empty;
    }

    private static string ExigirTexto(string valor, string nomeParametro)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException("O campo deve ser informado.", nomeParametro);
        }

        return valor.Trim();
    }

    private static string ValidarCep(string cep)
    {
        var cepNumerico = Regex.Replace(cep ?? string.Empty, "\\D", string.Empty);

        if (cepNumerico.Length != 8)
        {
            throw new ArgumentException("O CEP deve conter 8 dígitos.", nameof(cep));
        }

        return $"{cepNumerico[..5]}-{cepNumerico[5..]}";
    }

    public string ObterEnderecoCompleto()
    {
        var complementoTexto = string.IsNullOrWhiteSpace(Complemento) ? string.Empty : $", {Complemento}";
        return $"{Logradouro}, {Numero}{complementoTexto} - {Bairro}, {Cidade}/{Estado} - CEP: {Cep}";
    }

    public override string ToString() => ObterEnderecoCompleto();
}
