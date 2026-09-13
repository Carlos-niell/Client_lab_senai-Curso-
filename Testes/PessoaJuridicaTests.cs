using ClientLab.Models;
using Xunit;

namespace ClientLab.Tests;

public class PessoaJuridicaTests
{
    private static Endereco CriarEnderecoPadrao() =>
        new(
            logradouro: "Avenida Central",
            numero: "500",
            bairro: "Centro Empresarial",
            cidade: "São Paulo",
            estado: "SP",
            cep: "01310-100",
            complemento: "Torre A, Sala 1502");

    [Fact]
    public void PagarImposto_DeveCalcularCincoPorCentoCorretamente()
    {
        // Arrange
        var pj = new PessoaJuridica(
            "ClientLab Tecnologia",
            CriarEnderecoPadrao(),
            "(11) 3333-4444",
            "11.222.333/0001-81",
            "ClientLab Tecnologia Ltda.");

        var valorBase = 10000.00m;

        // Act
        pj.PagarImposto(valorBase);

        // Assert
        Assert.Equal(10000.00m, pj.Valor);
        Assert.Equal(500.00m, pj.ValorImposto); // 5%
        Assert.Equal(10500.00m, pj.Total);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void PagarImposto_DeveLancarExcecao_QuandoValorForInvalido(decimal valorInvalido)
    {
        // Arrange
        var pj = new PessoaJuridica(
            "ClientLab Tecnologia",
            CriarEnderecoPadrao(),
            "(11) 3333-4444",
            "11.222.333/0001-81",
            "ClientLab Tecnologia Ltda.");

        // Act & Assert
        var erro = Assert.Throws<ArgumentException>(() => pj.PagarImposto(valorInvalido));
        Assert.Contains("maior que zero", erro.Message);
    }

    [Fact]
    public void Construtor_DeveCadastrarComSucesso_QuandoDadosForemValidos()
    {
        // Arrange & Act
        var endereco = CriarEnderecoPadrao();
        var pj = new PessoaJuridica(
            "ClientLab Tecnologia",
            endereco,
            "(11) 3333-4444",
            "11.222.333/0001-81",
            "ClientLab Tecnologia Ltda.");

        // Assert
        Assert.Equal("ClientLab Tecnologia", pj.Nome);
        Assert.Equal("ClientLab Tecnologia Ltda.", pj.RazaoSocial);
        Assert.Equal("11222333000181", pj.CNPJ);
        Assert.Equal("(11) 3333-4444", pj.Telefone);
        Assert.Equal(endereco, pj.Endereco);
    }

    [Theory]
    [InlineData("11.111.111/1111-11")]
    [InlineData("12.345.678/0001-00")]
    [InlineData("123")]
    [InlineData("")]
    public void Construtor_DeveLancarExcecao_QuandoCnpjForInvalido(string cnpjInvalido)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new PessoaJuridica(
                "Empresa Teste",
                CriarEnderecoPadrao(),
                "(11) 3333-4444",
                cnpjInvalido,
                "Razao Social Teste"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Construtor_DeveLancarExcecao_QuandoRazaoSocialForVazia(string razaoSocialInvalida)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new PessoaJuridica(
                "Empresa Teste",
                CriarEnderecoPadrao(),
                "(11) 3333-4444",
                "11.222.333/0001-81",
                razaoSocialInvalida));
    }
}
