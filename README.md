# 🏢 ClientLab - Sistema de Cadastro de Clientes (POO em C#)

Projeto desenvolvido para a atividade do curso **SENAI**, com foco na aplicação prática dos pilares da **Programação Orientada a Objetos (POO)** utilizando **C#** e **.NET 10**.

---

## 📌 Visão Geral do Projeto

O **ClientLab** é uma aplicação de console em C# que simula um sistema de gestão e validação cadastral de clientes, divididos entre **Pessoa Física** e **Pessoa Jurídica**. 

O projeto implementa regras de negócio estritas diretamente no domínio através de construtores com validações automáticas, garantindo que nenhum objeto seja instanciado em um estado inconsistente ou inválido.

---

## 📂 Estrutura de Pastas e Arquivos

```text
ClientLab/
├── Modelos/
│   ├── Pessoa.cs             # Classe base abstrata com dados comuns
│   ├── PessoaFisica.cs       # Especialização para Pessoa Física (CPF, maioridade)
│   └── PessoaJuridica.cs     # Especialização para Pessoa Jurídica (CNPJ, Razão Social)
├── Program.cs                # Ponto de entrada com demonstração e testes de validação
├── ClientLab.csproj          # Configurações do projeto .NET 10
├── .gitignore                # Arquivos ignorados no versionamento Git
└── README.md                 # Documentação completa do projeto
```

---

## 🏗️ Modelagem e Explicação das Classes

### 1. `Pessoa` (Classe Abstrata Base)
Localizada em: `Modelos/Pessoa.cs`

A classe `Pessoa` define o contrato e os atributos fundamentais que todo cliente deve possuir no sistema.

* **Modificador `abstract`**: Não permite instanciação direta (`new Pessoa(...)`), servindo exclusivamente como classe pai para as especializações.
* **Propriedades**:
  * `Nome` (`string`): Nome do cliente ou titular.
  * `Endereco` (`string`): Endereço completo.
  * `Telefone` (`string`): Número de telefone para contato.
* **Encapsulamento**: Todas as propriedades possuem `get; private set;`, impedindo alterações externas após a criação.
* **Validação `ExigirTexto`**: Método protegido e estático que valida se o texto não é nulo, vazio ou composto apenas de espaços em branco, lançando `ArgumentException` caso o campo não seja preenchido.

---

### 2. `PessoaFisica` (Classe Selada)
Localizada em: `Modelos/PessoaFisica.cs`

Herda de `Pessoa` e implementa os requisitos específicos para pessoas naturais.

* **Modificador `sealed`**: Impede derivação adicional, selando o comportamento da classe.
* **Propriedades Adicionais**:
  * `CPF` (`string`): Cadastro de Pessoa Física (armazenado apenas com dígitos numéricos).
  * `DataNascimento` (`DateOnly`): Data de nascimento do cliente.
* **Regras de Negócio e Validações**:
  * **Validação de CPF (`ValidarCpf`)**:
    * Remove formatação (pontos e traços) via Expressão Regular (`Regex`).
    * Exige exatamente 11 dígitos numéricos.
    * Rejeita sequências com dígitos repetidos (ex: `111.111.111-11`, `000.000.000-00`).
  * **Validação de Idade (`ValidarDataNascimento`)**:
    * Calcula a idade exata com base na data atual (`DateTime.Today`).
    * Exige idade **mínima de 18 anos**.
    * Rejeita datas futuras ou menores de idade, lançando `ArgumentException`.

---

### 3. `PessoaJuridica` (Classe Selada)
Localizada em: `Modelos/PessoaJuridica.cs`

Herda de `Pessoa` e implementa os requisitos para empresas e organizações.

* **Modificador `sealed`**: Impede que a classe seja herdada.
* **Propriedades Adicionais**:
  * `CNPJ` (`string`): Cadastro Nacional da Pessoa Jurídica (armazenado apenas com dígitos numéricos).
  * `RazaoSocial` (`string`): Nome oficial de registro da empresa.
* **Regras de Negócio e Validações**:
  * **Validação da Razão Social**: Utiliza o método herdado `ExigirTexto` para garantir que o campo esteja preenchido.
  * **Validação Completa de CNPJ (`ValidarCnpj` e `CalcularDigito`)**:
    * Remove caracteres não numéricos via `Regex`.
    * Verifica se o CNPJ possui exatamente 14 dígitos e descarta sequências repetidas.
    * **Algoritmo Módulo 11**: Realiza o cálculo matemático dos dois dígitos verificadores aplicando as matrizes de pesos ponderados oficiais da Receita Federal:
      * **1º Dígito**: pesos `[5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]`.
      * **2º Dígito**: pesos `[6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]`.
    * Se os dígitos calculados divergirem dos informados, lança `ArgumentException`.

---

### 4. `Program.cs` (Ponto de Entrada e Demonstração)
Arquivo principal da aplicação que executa o fluxo demonstrativo:

1. **Instanciação com Sucesso**:
   * Cria uma `PessoaFisica` com dados válidos e maior de idade.
   * Cria uma `PessoaJuridica` com CNPJ válido e razão social preenchida.
   * Exibe as informações cadastradas no console.
2. **Tratamento de Exceções (`try / catch`)**:
   * Tenta cadastrar uma `PessoaFisica` menor de idade e captura a `ArgumentException` exibindo a mensagem explicativa.
   * Tenta cadastrar uma `PessoaJuridica` com CNPJ inválido e captura o erro sem derrubar o programa.

---

## 💡 Pilares de POO Demonstrados

| Pilar | Como foi aplicado |
| :--- | :--- |
| **Abstração** | A classe `Pessoa` isola conceitos essenciais de um cliente genérico, servindo como modelo base sem permitir instâncias diretas. |
| **Encapsulamento** | Propriedades com `private set`, métodos utilitários privados/protegidos e validações automáticas no construtor que protegem o estado interno do objeto. |
| **Herança** | `PessoaFisica` e `PessoaJuridica` herdam atributos e métodos comuns de `Pessoa` (`base(...)`), evitando duplicação de código. |
| **Polimorfismo e Reutilização** | Reutilização de métodos de validação (`ExigirTexto`) e padronização do contrato de entidades. |

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
* [.NET SDK 10.0](https://dotnet.microsoft.com/download) (ou versão compatível)

### Passos para Execução
1. Clone o repositório ou abra a pasta do projeto:
   ```powershell
   git clone https://github.com/Carlos-niell/Client_lab_senai-Curso-.git
   cd ClientLab
   ```

2. Compile e execute o projeto via CLI:
   ```powershell
   dotnet run
   ```

3. Exemplo de saída esperada no console:
   ```text
   === ClientLab ===
   Pessoa física cadastrada: Ana Souza - CPF 52998224725
   Pessoa jurídica cadastrada: ClientLab Tecnologia Ltda. - CNPJ 11222333000181
   Erro de pessoa física: O cadastro permite apenas pessoas com idade igual ou superior a 18 anos. (Parameter 'dataNascimento')
   Erro de pessoa jurídica: O CNPJ informado não é válido. (Parameter 'cnpj')
   ```

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem**: C#
- **Plataforma**: .NET 10.0 (Console Application)
- **IDE recomendada**: Visual Studio Code / Visual Studio 2022+ com C# Dev Kit

---

## 👤 Autor

Desenvolvido por **Carlos Niell** durante o curso **SENAI**.