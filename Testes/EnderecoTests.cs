using ClientLab.Models;
using Xunit;

namespace ClientLab.Tests;

public class EnderecoTests
{
    [Fact]
    public void Construtor_DeveCriarEnderecoComSucesso_QuandoDadosValidos()
    {
        // Arrange & Act
        var endereco = new Endereco(
            logradouro: "Rua das Flores",
            numero: "100",
            bairro: "Jardim Primavera",
            cidade: "São Paulo",
            estado: "SP",
            cep: "01001-000",
            complemento: "Apto 42");

        // Assert
        Assert.Equal("Rua das Flores", endereco.Logradouro);
        Assert.Equal("100", endereco.Numero);
        Assert.Equal("Jardim Primavera", endereco.Bairro);
        Assert.Equal("São Paulo", endereco.Cidade);
        Assert.Equal("SP", endereco.Estado);
        Assert.Equal("01001-000", endereco.Cep);
        Assert.Equal("Apto 42", endereco.Complemento);
    }

    [Fact]
    public void ObterEnderecoCompleto_DeveFormatarStringCorretamente()
    {
        // Arrange
        var endereco = new Endereco(
            logradouro: "Rua das Flores",
            numero: "100",
            bairro: "Jardim Primavera",
            cidade: "São Paulo",
            estado: "SP",
            cep: "01001000",
            complemento: "Apto 42");

        // Act
        var formatado = endereco.ObterEnderecoCompleto();

        // Assert
        Assert.Equal("Rua das Flores, 100, Apto 42 - Jardim Primavera, São Paulo/SP - CEP: 01001-000", formatado);
        Assert.Equal(formatado, endereco.ToString());
    }

    [Theory]
    [InlineData("123")]
    [InlineData("123456789")]
    [InlineData("")]
    public void Construtor_DeveLancarExcecao_QuandoCepForInvalido(string cepInvalido)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Endereco(
                "Rua A",
                "10",
                "Bairro B",
                "Cidade C",
                "SP",
                cepInvalido));
    }

    [Theory]
    [InlineData("", "10", "Bairro", "Cidade", "SP", "01001-000")]
    [InlineData("Rua", "", "Bairro", "Cidade", "SP", "01001-000")]
    [InlineData("Rua", "10", "", "Cidade", "SP", "01001-000")]
    [InlineData("Rua", "10", "Bairro", "", "SP", "01001-000")]
    [InlineData("Rua", "10", "Bairro", "Cidade", "", "01001-000")]
    public void Construtor_DeveLancarExcecao_QuandoCampoObrigatorioEstiverVazio(
        string logradouro,
        string numero,
        string bairro,
        string cidade,
        string estado,
        string cep)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Endereco(
                logradouro,
                numero,
                bairro,
                cidade,
                estado,
                cep));
    }
}
