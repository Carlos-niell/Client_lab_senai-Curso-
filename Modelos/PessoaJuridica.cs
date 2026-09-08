using System.Text.RegularExpressions;

namespace ClientLab.Models;

public sealed class PessoaJuridica : Pessoa
{
    public string CNPJ { get; private set; }
    public string RazaoSocial { get; private set; }

    public PessoaJuridica(
        string nome,
        string endereco,
        string telefone,
        string cnpj,
        string razaoSocial)
        : base(nome, endereco, telefone)
    {
        CNPJ = ValidarCnpj(cnpj);
        RazaoSocial = ExigirTexto(razaoSocial, nameof(razaoSocial));
    }

    private static string ValidarCnpj(string cnpj)
    {
        var cnpjNumerico = Regex.Replace(cnpj ?? string.Empty, "\\D", string.Empty);

        if (cnpjNumerico.Length != 14 || cnpjNumerico.All(caractere => caractere == cnpjNumerico[0]))
        {
            throw new ArgumentException("O CNPJ deve conter 14 dígitos válidos.", nameof(cnpj));
        }

        var primeiroDigito = CalcularDigito(cnpjNumerico[..12]);
        var segundoDigito = CalcularDigito(cnpjNumerico[..13]);

        if (cnpjNumerico[12] - '0' != primeiroDigito || cnpjNumerico[13] - '0' != segundoDigito)
        {
            throw new ArgumentException("O CNPJ informado não é válido.", nameof(cnpj));
        }

        return cnpjNumerico;
    }

    private static int CalcularDigito(string baseCnpj)
    {
        var pesos = baseCnpj.Length == 12
            ? new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 }
            : new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var resto = baseCnpj.Select((digito, indice) => (digito - '0') * pesos[indice]).Sum() % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}