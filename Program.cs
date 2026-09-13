using System.Globalization;
using ClientLab.Models;
using ClientLab.Services;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

Console.WriteLine("==========================================================");
Console.WriteLine("        🏢 ClientLab - Sistema de Gestão de Clientes       ");
Console.WriteLine("==========================================================\n");

// 1. Instanciação e Cálculo de Imposto para Pessoa Física (3%)
var enderecoPf = new Endereco(
    logradouro: "Rua das Flores",
    numero: "100",
    bairro: "Jardim Primavera",
    cidade: "São Paulo",
    estado: "SP",
    cep: "01001-000",
    complemento: "Apto 42");

var pessoaFisica = new PessoaFisica(
    nome: "Carlos Daniel",
    endereco: enderecoPf,
    telefone: "(11) 99999-0000",
    cpf: "529.982.247-25",
    dataNascimento: new DateOnly(1990, 5, 12));

pessoaFisica.PagarImposto(1000.00m);

Console.WriteLine("--- 👤 Cadastro de Pessoa Física ---");
Console.WriteLine($"Nome:             {pessoaFisica.Nome}");
Console.WriteLine($"CPF:              {pessoaFisica.CPF}");
Console.WriteLine($"Nascimento:       {pessoaFisica.DataNascimento:dd/MM/yyyy}");
Console.WriteLine($"Telefone:         {pessoaFisica.Telefone}");
Console.WriteLine($"Endereço:         {pessoaFisica.Endereco}");
Console.WriteLine($"Valor Base:       {pessoaFisica.Valor:C2}");
Console.WriteLine($"Imposto (3%):     {pessoaFisica.ValorImposto:C2}");
Console.WriteLine($"Total a Pagar:    {pessoaFisica.Total:C2}\n");

// 2. Instanciação e Cálculo de Imposto para Pessoa Jurídica (5%)
var enderecoPj = new Endereco(
    logradouro: "Avenida Central",
    numero: "500",
    bairro: "Centro Empresarial",
    cidade: "São Paulo",
    estado: "SP",
    cep: "01310-100",
    complemento: "Torre A, Sala 1502");

var pessoaJuridica = new PessoaJuridica(
    nome: "ClientLab Tecnologia",
    endereco: enderecoPj,
    telefone: "(11) 3333-4444",
    cnpj: "11.222.333/0001-81",
    razaoSocial: "ClientLab Tecnologia Ltda.");

pessoaJuridica.PagarImposto(10000.00m);

Console.WriteLine("--- 🏢 Cadastro de Pessoa Jurídica ---");
Console.WriteLine($"Nome Fantasia:    {pessoaJuridica.Nome}");
Console.WriteLine($"Razão Social:     {pessoaJuridica.RazaoSocial}");
Console.WriteLine($"CNPJ:             {pessoaJuridica.CNPJ}");
Console.WriteLine($"Telefone:         {pessoaJuridica.Telefone}");
Console.WriteLine($"Endereço:         {pessoaJuridica.Endereco}");
Console.WriteLine($"Valor Base:       {pessoaJuridica.Valor:C2}");
Console.WriteLine($"Imposto (5%):     {pessoaJuridica.ValorImposto:C2}");
Console.WriteLine($"Total a Pagar:    {pessoaJuridica.Total:C2}\n");

// 3. Gravação e Leitura de Arquivos .TXT utilizando a biblioteca do .NET (System.IO)
Console.WriteLine("--- 📁 Manipulação de Arquivos .TXT (.NET System.IO) ---");

// Salva o arquivo de Pessoa Física com o nome do cliente: 'Carlos Daniel.txt'
var caminhoArquivoPf = ClienteArquivoService.SalvarEmArquivoTxt(pessoaFisica);
Console.WriteLine($"[Gravado] Arquivo PF gerado: {Path.GetFileName(caminhoArquivoPf)}");
Console.WriteLine($"          Caminho: {caminhoArquivoPf}");

// Salva o arquivo de Pessoa Jurídica com o nome da empresa/cliente: 'ClientLab Tecnologia.txt'
var caminhoArquivoPj = ClienteArquivoService.SalvarEmArquivoTxt(pessoaJuridica);
Console.WriteLine($"[Gravado] Arquivo PJ gerado: {Path.GetFileName(caminhoArquivoPj)}");
Console.WriteLine($"          Caminho: {caminhoArquivoPj}\n");

// Demonstração da leitura de arquivo do disco utilizando a biblioteca do .NET
Console.WriteLine("--- 📖 Conteúdo Lido do Arquivo TXT (Exemplo PF) ---");
var conteudoLidoPf = ClienteArquivoService.LerArquivoTxt(caminhoArquivoPf);
Console.WriteLine(conteudoLidoPf);

// 4. Testes de Validações e Regras de Negócio (Tratamento de Exceções)
Console.WriteLine("--- 🧪 Validações de Regras de Negócio ---");

try
{
    _ = new PessoaFisica(
        "Menor de Idade",
        enderecoPf,
        "(11) 90000-0000",
        "529.982.247-25",
        DateOnly.FromDateTime(DateTime.Today.AddYears(-17)));
}
catch (ArgumentException erro)
{
    Console.WriteLine($"[Esperado] Validação Idade: {erro.Message}");
}

try
{
    _ = new PessoaJuridica(
        "CNPJ Inválido",
        enderecoPj,
        "(11) 3000-0000",
        "12.345.678/0001-00",
        "Empresa com CNPJ Inválido");
}
catch (ArgumentException erro)
{
    Console.WriteLine($"[Esperado] Validação CNPJ:  {erro.Message}");
}

try
{
    pessoaFisica.PagarImposto(0);
}
catch (ArgumentException erro)
{
    Console.WriteLine($"[Esperado] Validação Valor: {erro.Message}");
}

Console.WriteLine("\n==========================================================");
Console.WriteLine("            Execução concluída com sucesso!               ");
Console.WriteLine("==========================================================");