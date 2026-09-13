using System.Text.RegularExpressions;

namespace ClientLab.Models;

public sealed class PessoaJuridica : Pessoa
{
    public string CNPJ { get; private set; }
    public string RazaoSocial { get; private set; }

    public PessoaJuridica(
        string nome,
        Endereco endereco,
        string telefone,
        string cnpj,
        string razaoSocial)
        : base(nome, endereco, telefone)
    {
        CNPJ = ValidarCnpj(cnpj);
        RazaoSocial = ExigirTexto(razaoSocial, nameof(razaoSocial));
    }

    public PessoaJuridica(
        string nome,
        string logradouro,
        string numero,
        string bairro,
        string cidade,
        string estado,
        string cep,
        string telefone,
        string cnpj,
        string razaoSocial,
        string complemento = "")
        : this(
            nome,
            new Endereco(logradouro, numero, bairro, cidade, estado, cep, complemento),
            telefone,
            cnpj,
            razaoSocial)
    {
    }

    public override void PagarImposto(decimal valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor para cálculo do imposto deve ser maior que zero.", nameof(valor));
        }

        Valor = valor;
        ValorImposto = valor * 0.05m;
        Total = valor + ValorImposto;
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