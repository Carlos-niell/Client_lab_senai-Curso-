using System.Text.RegularExpressions;

namespace ClientLab.Models;

public sealed class PessoaFisica : Pessoa
{
    public string CPF { get; private set; }
    public DateOnly DataNascimento { get; private set; }

    public PessoaFisica(
        string nome,
        Endereco endereco,
        string telefone,
        string cpf,
        DateOnly dataNascimento)
        : base(nome, endereco, telefone)
    {
        CPF = ValidarCpf(cpf);
        DataNascimento = ValidarDataNascimento(dataNascimento);
    }

    public PessoaFisica(
        string nome,
        string logradouro,
        string numero,
        string bairro,
        string cidade,
        string estado,
        string cep,
        string telefone,
        string cpf,
        DateOnly dataNascimento,
        string complemento = "")
        : this(
            nome,
            new Endereco(logradouro, numero, bairro, cidade, estado, cep, complemento),
            telefone,
            cpf,
            dataNascimento)
    {
    }

    public override void PagarImposto(decimal valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor para cálculo do imposto deve ser maior que zero.", nameof(valor));
        }

        Valor = valor;
        ValorImposto = valor * 0.03m;
        Total = valor + ValorImposto;
    }

    private static string ValidarCpf(string cpf)
    {
        var cpfNumerico = Regex.Replace(cpf ?? string.Empty, "\\D", string.Empty);

        if (cpfNumerico.Length != 11 || cpfNumerico.All(caractere => caractere == cpfNumerico[0]))
        {
            throw new ArgumentException("O CPF deve conter 11 dígitos válidos.", nameof(cpf));
        }

        return cpfNumerico;
    }

    private static DateOnly ValidarDataNascimento(DateOnly dataNascimento)
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var idade = hoje.Year - dataNascimento.Year;

        if (dataNascimento > hoje || idade < 18 || dataNascimento.AddYears(idade) > hoje)
        {
            throw new ArgumentException("O cadastro permite apenas pessoas com idade igual ou superior a 18 anos.", nameof(dataNascimento));
        }

        return dataNascimento;
    }
}