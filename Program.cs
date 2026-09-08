using ClientLab.Models;

Console.WriteLine("=== ClientLab ===");

var pessoaFisica = new PessoaFisica(
    "Ana Souza",
    "Rua das Flores, 100",
    "(11) 99999-0000",
    "529.982.247-25",
    new DateOnly(1990, 5, 12));

var pessoaJuridica = new PessoaJuridica(
    "ClientLab Tecnologia",
    "Avenida Central, 500",
    "(11) 3333-4444",
    "11.222.333/0001-81",
    "ClientLab Tecnologia Ltda.");

Console.WriteLine($"Pessoa física cadastrada: {pessoaFisica.Nome} - CPF {pessoaFisica.CPF}");
Console.WriteLine($"Pessoa jurídica cadastrada: {pessoaJuridica.RazaoSocial} - CNPJ {pessoaJuridica.CNPJ}");

try
{
    _ = new PessoaFisica(
        "Cadastro inválido",
        "Rua A, 1",
        "(11) 0000-0000",
        "12345678909",
        DateOnly.FromDateTime(DateTime.Today.AddYears(-17)));
}
catch (ArgumentException erro)
{
    Console.WriteLine($"Erro de pessoa física: {erro.Message}");
}

try
{
    _ = new PessoaJuridica(
        "Cadastro inválido",
        "Rua B, 2",
        "(11) 0000-0000",
        "12.345.678/0001-00",
        "Empresa inválida");
}
catch (ArgumentException erro)
{
    Console.WriteLine($"Erro de pessoa jurídica: {erro.Message}");
}