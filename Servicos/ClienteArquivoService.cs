using System.Globalization;
using System.Text;
using ClientLab.Models;

namespace ClientLab.Services;


public static class ClienteArquivoService
{
    private static readonly CultureInfo CulturaBrasil = new("pt-BR");


    public static string ObterNomeArquivo(Pessoa pessoa)
    {
        ArgumentNullException.ThrowIfNull(pessoa);

        var nomeSanitizado = SanitizarNomeArquivo(pessoa.Nome);
        return $"{nomeSanitizado}.txt";
    }

    public static string FormatarConteudoTxt(Pessoa pessoa)
    {
        ArgumentNullException.ThrowIfNull(pessoa);

        var sb = new StringBuilder();
        sb.AppendLine("==================================================================");
        sb.AppendLine("                  CLIENTLAB - REGISTRO DE CLIENTE                 ");
        sb.AppendLine("==================================================================");
        sb.AppendLine($"Data/Hora da Gravação: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        sb.AppendLine();

        if (pessoa is PessoaFisica pf)
        {
            sb.AppendLine("--- 👤 DADOS DE PESSOA FÍSICA ---");
            sb.AppendLine($"Nome:               {pf.Nome}");
            sb.AppendLine($"CPF:                {pf.CPF}");
            sb.AppendLine($"Data de Nascimento: {pf.DataNascimento:dd/MM/yyyy}");
            sb.AppendLine($"Telefone:           {pf.Telefone}");
            sb.AppendLine();
            sb.AppendLine("--- 📍 ENDEREÇO ---");
            sb.AppendLine($"Logradouro:         {pf.Endereco.Logradouro}, Nº {pf.Endereco.Numero}");
            if (!string.IsNullOrWhiteSpace(pf.Endereco.Complemento))
            {
                sb.AppendLine($"Complemento:        {pf.Endereco.Complemento}");
            }
            sb.AppendLine($"Bairro:             {pf.Endereco.Bairro}");
            sb.AppendLine($"Cidade/UF:          {pf.Endereco.Cidade}/{pf.Endereco.Estado}");
            sb.AppendLine($"CEP:                {pf.Endereco.Cep}");
            sb.AppendLine();
            sb.AppendLine("--- 💰 INFORMAÇÕES FINANCEIRAS ---");
            sb.AppendLine($"Valor Base:         {pf.Valor.ToString("C2", CulturaBrasil)}");
            sb.AppendLine($"Alíquota Imposto:   3%");
            sb.AppendLine($"Valor do Imposto:   {pf.ValorImposto.ToString("C2", CulturaBrasil)}");
            sb.AppendLine($"Total com Imposto:  {pf.Total.ToString("C2", CulturaBrasil)}");
        }
        else if (pessoa is PessoaJuridica pj)
        {
            sb.AppendLine("--- 🏢 DADOS DE PESSOA JURÍDICA ---");
            sb.AppendLine($"Nome Fantasia:      {pj.Nome}");
            sb.AppendLine($"Razão Social:       {pj.RazaoSocial}");
            sb.AppendLine($"CNPJ:               {pj.CNPJ}");
            sb.AppendLine($"Telefone:           {pj.Telefone}");
            sb.AppendLine();
            sb.AppendLine("--- 📍 ENDEREÇO ---");
            sb.AppendLine($"Logradouro:         {pj.Endereco.Logradouro}, Nº {pj.Endereco.Numero}");
            if (!string.IsNullOrWhiteSpace(pj.Endereco.Complemento))
            {
                sb.AppendLine($"Complemento:        {pj.Endereco.Complemento}");
            }
            sb.AppendLine($"Bairro:             {pj.Endereco.Bairro}");
            sb.AppendLine($"Cidade/UF:          {pj.Endereco.Cidade}/{pj.Endereco.Estado}");
            sb.AppendLine($"CEP:                {pj.Endereco.Cep}");
            sb.AppendLine();
            sb.AppendLine("--- 💰 INFORMAÇÕES FINANCEIRAS ---");
            sb.AppendLine($"Valor Base:         {pj.Valor.ToString("C2", CulturaBrasil)}");
            sb.AppendLine($"Alíquota Imposto:   5%");
            sb.AppendLine($"Valor do Imposto:   {pj.ValorImposto.ToString("C2", CulturaBrasil)}");
            sb.AppendLine($"Total com Imposto:  {pj.Total.ToString("C2", CulturaBrasil)}");
        }

        sb.AppendLine();
        sb.AppendLine("==================================================================");
        sb.AppendLine("                     FIM DO REGISTRO                              ");
        sb.AppendLine("==================================================================");

        return sb.ToString();
    }


    public static string SalvarEmArquivoTxt(Pessoa pessoa, string? diretorioDestino = null)
    {
        ArgumentNullException.ThrowIfNull(pessoa);

        var diretorio = string.IsNullOrWhiteSpace(diretorioDestino)
            ? Directory.GetCurrentDirectory()
            : diretorioDestino;

        if (!Directory.Exists(diretorio))
        {
            Directory.CreateDirectory(diretorio);
        }

        var nomeArquivo = ObterNomeArquivo(pessoa);
        var caminhoCompleto = Path.Combine(diretorio, nomeArquivo);
        var conteudo = FormatarConteudoTxt(pessoa);

        File.WriteAllText(caminhoCompleto, conteudo, Encoding.UTF8);

        return Path.GetFullPath(caminhoCompleto);
    }

    public static string LerArquivoTxt(string caminhoArquivo)
    {
        if (string.IsNullOrWhiteSpace(caminhoArquivo))
        {
            throw new ArgumentException("O caminho do arquivo deve ser informado.", nameof(caminhoArquivo));
        }

        if (!File.Exists(caminhoArquivo))
        {
            throw new FileNotFoundException($"O arquivo '{caminhoArquivo}' não foi encontrado.");
        }

        return File.ReadAllText(caminhoArquivo, Encoding.UTF8);
    }

    private static string SanitizarNomeArquivo(string nome)
    {
        var invalidos = Path.GetInvalidFileNameChars();
        var sb = new StringBuilder(nome.Length);

        foreach (var c in nome)
        {
            if (!invalidos.Contains(c))
            {
                sb.Append(c);
            }
        }

        var resultado = sb.ToString().Trim();
        return string.IsNullOrEmpty(resultado) ? "cliente_sem_nome" : resultado;
    }
}
