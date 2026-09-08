using System.Text.RegularExpressions;

namespace ClientLab.Models;

public sealed class PessoaFisica : Pessoa
{
    public string CPF { get; private set; }
    public DateOnly DataNascimento { get; private set; }

    public PessoaFisica(
        string nome,
        string endereco,
        string telefone,
        string cpf,
        DateOnly dataNascimento)
        : base(nome, endereco, telefone)
    {
        CPF = ValidarCpf(cpf);
        DataNascimento = ValidarDataNascimento(dataNascimento);
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