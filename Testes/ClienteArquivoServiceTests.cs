using ClientLab.Models;
using ClientLab.Services;
using Xunit;

namespace ClientLab.Tests;

public class ClienteArquivoServiceTests : IDisposable
{
    private readonly string _diretorioTeste;

    public ClienteArquivoServiceTests()
    {
        _diretorioTeste = Path.Combine(Path.GetTempPath(), "ClientLab_Tests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_diretorioTeste);
    }

    public void Dispose()
    {
        if (Directory.Exists(_diretorioTeste))
        {
            try
            {
                Directory.Delete(_diretorioTeste, recursive: true);
            }
            catch
            {
                // Ignora falhas de limpeza em arquivos temporários
            }
        }
    }

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
    public void SalvarEmArquivoTxt_PessoaFisica_DeveCriarArquivoComNomeDoCliente()
    {
        // Arrange
        var pf = new PessoaFisica(
            nome: "Carlos Daniel",
            endereco: CriarEnderecoPadrao(),
            telefone: "(11) 99999-0000",
            cpf: "529.982.247-25",
            dataNascimento: new DateOnly(1990, 5, 12));
        pf.PagarImposto(1000.00m);

        // Act
        var caminhoGerado = ClienteArquivoService.SalvarEmArquivoTxt(pf, _diretorioTeste);

        // Assert
        var nomeEsperado = "Carlos Daniel.txt";
        Assert.True(File.Exists(caminhoGerado));
        Assert.EndsWith(nomeEsperado, caminhoGerado);

        var conteudo = File.ReadAllText(caminhoGerado);
        Assert.Contains("Carlos Daniel", conteudo);
        Assert.Contains("52998224725", conteudo);
        Assert.Contains("3%", conteudo);
        Assert.Contains("R$ 1.030,00", conteudo);
    }

    [Fact]
    public void SalvarEmArquivoTxt_PessoaJuridica_DeveCriarArquivoComNomeDoCliente()
    {
        // Arrange
        var pj = new PessoaJuridica(
            nome: "ClientLab Tecnologia",
            endereco: CriarEnderecoPadrao(),
            telefone: "(11) 3333-4444",
            cnpj: "11.222.333/0001-81",
            razaoSocial: "ClientLab Tecnologia Ltda.");
        pj.PagarImposto(10000.00m);

        // Act
        var caminhoGerado = ClienteArquivoService.SalvarEmArquivoTxt(pj, _diretorioTeste);

        // Assert
        var nomeEsperado = "ClientLab Tecnologia.txt";
        Assert.True(File.Exists(caminhoGerado));
        Assert.EndsWith(nomeEsperado, caminhoGerado);

        var conteudo = File.ReadAllText(caminhoGerado);
        Assert.Contains("ClientLab Tecnologia", conteudo);
        Assert.Contains("ClientLab Tecnologia Ltda.", conteudo);
        Assert.Contains("11222333000181", conteudo);
        Assert.Contains("5%", conteudo);
        Assert.Contains("R$ 10.500,00", conteudo);
    }

    [Fact]
    public void LerArquivoTxt_DeveRetornarConteudoCorreto_AposGravacao()
    {
        // Arrange
        var pf = new PessoaFisica(
            nome: "Ana Souza",
            endereco: CriarEnderecoPadrao(),
            telefone: "(11) 98888-7777",
            cpf: "529.982.247-25",
            dataNascimento: new DateOnly(1995, 3, 20));
        pf.PagarImposto(2500.00m);

        var caminhoGerado = ClienteArquivoService.SalvarEmArquivoTxt(pf, _diretorioTeste);

        // Act
        var conteudoLido = ClienteArquivoService.LerArquivoTxt(caminhoGerado);

        // Assert
        Assert.NotEmpty(conteudoLido);
        Assert.Contains("Ana Souza", conteudoLido);
        Assert.Contains("Ana Souza.txt", Path.GetFileName(caminhoGerado));
    }

    [Fact]
    public void SalvarEmArquivoTxt_DeveLancarExcecao_QuandoPessoaForNula()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => ClienteArquivoService.SalvarEmArquivoTxt(null!));
    }

    [Fact]
    public void LerArquivoTxt_DeveLancarExcecao_QuandoArquivoNaoExistir()
    {
        // Arrange
        var caminhoInexistente = Path.Combine(_diretorioTeste, "arquivo_que_nao_existe.txt");

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => ClienteArquivoService.LerArquivoTxt(caminhoInexistente));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void LerArquivoTxt_DeveLancarExcecao_QuandoCaminhoForVazio(string caminhoInvalido)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => ClienteArquivoService.LerArquivoTxt(caminhoInvalido));
    }
}
