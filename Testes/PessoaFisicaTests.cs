using ClientLab.Models;
using Xunit;

namespace ClientLab.Tests;

public class PessoaFisicaTests
{
    private static Endereco CriarEnderecoPadrao() =>
        new(
            logradouro: "Rua das Flores",
            numero: "100",
            bairro: "Jardim Primavera",
            cidade: "São Paulo",
            estado: "SP",
            cep: "01001-000",
            complemento: "Apto 42");

    [Fact]
    public void PagarImposto_DeveCalcularTresPorCentoCorretamente()
    {
        // Arrange
        var pf = new PessoaFisica(
            "Carlos Daniel",
            CriarEnderecoPadrao(),
            "(11) 99999-0000",
            "529.982.247-25",
            new DateOnly(1990, 5, 12));

        var valorBase = 1000.00m;

        // Act
        pf.PagarImposto(valorBase);

        // Assert
        Assert.Equal(1000.00m, pf.Valor);
        Assert.Equal(30.00m, pf.ValorImposto); // 3%
        Assert.Equal(1030.00m, pf.Total);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void PagarImposto_DeveLancarExcecao_QuandoValorForInvalido(decimal valorInvalido)
    {
        // Arrange
        var pf = new PessoaFisica(
            "Carlos Daniel",
            CriarEnderecoPadrao(),
            "(11) 99999-0000",
            "529.982.247-25",
            new DateOnly(1990, 5, 12));

        // Act & Assert
        var erro = Assert.Throws<ArgumentException>(() => pf.PagarImposto(valorInvalido));
        Assert.Contains("maior que zero", erro.Message);
    }

    [Fact]
    public void Construtor_DeveCadastrarComSucesso_QuandoDadosForemValidos()
    {
        // Arrange & Act
        var endereco = CriarEnderecoPadrao();
        var pf = new PessoaFisica(
            "Carlos Daniel",
            endereco,
            "(11) 99999-0000",
            "529.982.247-25",
            new DateOnly(1990, 5, 12));

        // Assert
        Assert.Equal("Carlos Daniel", pf.Nome);
        Assert.Equal("52998224725", pf.CPF);
        Assert.Equal(new DateOnly(1990, 5, 12), pf.DataNascimento);
        Assert.Equal("(11) 99999-0000", pf.Telefone);
        Assert.Equal(endereco, pf.Endereco);
    }

    [Fact]
    public void Construtor_DeveLancarExcecao_QuandoMenorDeIdade()
    {
        // Arrange & Act & Assert
        var dataMenorDeIdade = DateOnly.FromDateTime(DateTime.Today.AddYears(-17));

        var erro = Assert.Throws<ArgumentException>(() =>
            new PessoaFisica(
                "Menor de Idade",
                CriarEnderecoPadrao(),
                "(11) 99999-0000",
                "529.982.247-25",
                dataMenorDeIdade));

        Assert.Contains("18 anos", erro.Message);
    }

    [Theory]
    [InlineData("111.111.111-11")]
    [InlineData("123")]
    [InlineData("")]
    public void Construtor_DeveLancarExcecao_QuandoCpfForInvalido(string cpfInvalido)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new PessoaFisica(
                "Nome Teste",
                CriarEnderecoPadrao(),
                "(11) 99999-0000",
                cpfInvalido,
                new DateOnly(1990, 1, 1)));
    }
}
