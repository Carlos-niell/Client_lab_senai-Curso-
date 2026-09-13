# 🏢 ClientLab - Sistema de Cadastro e Gestão de Clientes (POO em C#)

Projeto desenvolvido para a atividade do curso **SENAI**, com foco na aplicação prática dos pilares da **Programação Orientada a Objetos (POO)** utilizando **C#** e **.NET 10**.

---

## 📌 Visão Geral do Projeto

O **ClientLab** é uma aplicação de console em C# que gerencia o cadastro e operações financeiras de clientes, divididos entre **Pessoa Física** e **Pessoa Jurídica**. 

Nesta versão evoluída, o sistema implementa:
1. **Composição e Modelagem de Endereços**: Uma classe `Endereco` estruturada com atributos completos (logradouro, número, complemento, bairro, cidade, estado e CEP) e validações.
2. **Cálculo Polimórfico de Impostos**:
   - **Pessoa Física**: cálculo de imposto de **3%** sobre o valor base.
   - **Pessoa Jurídica**: cálculo de imposto de **5%** sobre o valor base.
3. **Controle Financeiro de Transações**: Armazenamento e exibição de `Valor`, `ValorImposto` e `Total`.
4. **Validações Cadastrais Rígidas**: Validação completa de CPF, CNPJ (algoritmo Módulo 11) e verificação de maioridade (18+ anos).

---

## 📂 Estrutura de Pastas e Arquivos

```text
ClientLab/
├── Modelos/
│   ├── Endereco.cs           # Modelo de valor estruturado para endereço com validações
│   ├── Pessoa.cs             # Classe base abstrata com dados comuns e contrato PagarImposto
│   ├── PessoaFisica.cs       # Especialização para Pessoa Física (CPF, maioridade, imposto 3%)
│   └── PessoaJuridica.cs     # Especialização para Pessoa Jurídica (CNPJ, Razão Social, imposto 5%)
├── Program.cs                # Ponto de entrada com demonstração completa e testes
├── ClientLab.csproj          # Configurações do projeto .NET 10
├── .gitignore                # Arquivos ignorados no versionamento Git
└── README.md                 # Documentação completa do projeto
```

---

## 🏗️ Modelagem e Explicação das Classes

### 1. `Endereco` (Classe de Modelo)
Localizada em: `Modelos/Endereco.cs`

Modela o endereço físico do cliente com alto grau de detalhamento e validação.

* **Propriedades**:
  * `Logradouro` (`string`): Rua, avenida, praça, etc.
  * `Numero` (`string`): Número do imóvel.
  * `Complemento` (`string`): Informação adicional (apto, bloco, sala).
  * `Bairro` (`string`): Bairro da localidade.
  * `Cidade` (`string`): Município.
  * `Estado` (`string`): UF/Estado.
  * `Cep` (`string`): Código de Endereçamento Postal formatado (`00000-000`).
* **Regras de Negócio**:
  * Valida preenchimento obrigatório de todos os campos principais.
  * Valida se o CEP possui exatamente 8 dígitos numéricos.
  * Método `ObterEnderecoCompleto()` e sobreposição de `ToString()` para exibição padronizada.

---

### 2. `Pessoa` (Classe Abstrata Base)
Localizada em: `Modelos/Pessoa.cs`

Define o contrato e os atributos fundamentais comuns a todos os clientes.

* **Modificador `abstract`**: Não permite instanciação direta (`new Pessoa(...)`).
* **Propriedades**:
  * `Nome` (`string`): Nome do cliente ou titular.
  * `Endereco` (`Endereco`): Objeto estruturado de endereço (Composição).
  * `Telefone` (`string`): Número de telefone para contato.
  * `Valor` (`decimal`): Valor base informado para cálculo tributário.
  * `ValorImposto` (`decimal`): Valor calculado do imposto devido.
  * `Total` (`decimal`): Valor total com o imposto incluso (`Valor + ValorImposto`).
* **Método Abstrato**:
  * `public abstract void PagarImposto(decimal valor)`: Contrato obrigatório implementado por cada subclasse.

---

### 3. `PessoaFisica` (Classe Selada)
Localizada em: `Modelos/PessoaFisica.cs`

Herda de `Pessoa` e implementa as especificidades de pessoas naturais.

* **Modificador `sealed`**: Impede derivação adicional.
* **Propriedades Adicionais**:
  * `CPF` (`string`): Cadastro de Pessoa Física (armazenado apenas com dígitos numéricos).
  * `DataNascimento` (`DateOnly`): Data de nascimento do titular.
* **Regras de Negócio e Validações**:
  * **Cálculo de Imposto (`PagarImposto`)**:
    * Alíquota de **3%** (`valor * 0.03m`).
    * `Total` = `valor + ValorImposto`.
  * **Validação de CPF (`ValidarCpf`)**: Exige 11 dígitos numéricos e rejeita dígitos repetidos.
  * **Validação de Idade (`ValidarDataNascimento`)**: Exige idade **mínima de 18 anos**.

---

### 4. `PessoaJuridica` (Classe Selada)
Localizada em: `Modelos/PessoaJuridica.cs`

Herda de `Pessoa` e implementa as especificidades para empresas e organizações.

* **Modificador `sealed`**: Impede herança da classe.
* **Propriedades Adicionais**:
  * `CNPJ` (`string`): Cadastro Nacional da Pessoa Jurídica (armazenado apenas com dígitos numéricos).
  * `RazaoSocial` (`string`): Razão social registrada.
* **Regras de Negócio e Validações**:
  * **Cálculo de Imposto (`PagarImposto`)**:
    * Alíquota de **5%** (`valor * 0.05m`).
    * `Total` = `valor + ValorImposto`.
  * **Validação Completa de CNPJ (`ValidarCnpj` e `CalcularDigito`)**:
    * Algoritmo Módulo 11 com pesos oficiais da Receita Federal para os dois dígitos verificadores.

---

## 💡 Pilares de POO Demonstrados

| Pilar | Aplicação Prática no Projeto |
| :--- | :--- |
| **Abstração** | A classe `Pessoa` isola conceitos essenciais de um cliente genérico e define o contrato `PagarImposto`. |
| **Encapsulamento** | Propriedades com `get; private set;` / `protected set;` e validações automáticas que protegem o estado interno dos objetos. |
| **Herança** | `PessoaFisica` e `PessoaJuridica` herdam atributos comuns, métodos e propriedades financeiras de `Pessoa` via `base(...)`. |
| **Polimorfismo** | O método abstrato `PagarImposto` é sobrescrito (`override`) com regras fiscais distintas para PF (3%) e PJ (5%). |
| **Composição** | `Pessoa` possui uma instância de `Endereco`, desacoplando a estrutura de localização do cadastro geral. |

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
* [.NET SDK 10.0](https://dotnet.microsoft.com/download) (ou compatível)

### Passos para Execução
1. Clone o repositório ou navegue até a pasta do projeto:
   ```powershell
   git clone https://github.com/Carlos-niell/Client_lab_senai-Curso-.git
   cd ClientLab
   ```

2. Compile e execute a aplicação:
   ```powershell
   dotnet run
   ```

3. Exemplo de saída no console:
   ```text
   ==========================================================
           🏢 ClientLab - Sistema de Gestão de Clientes       
   ==========================================================

   --- 👤 Cadastro de Pessoa Física ---
   Nome:             Carlos Daniel
   CPF:              52998224725
   Nascimento:       12/05/1990
   Telefone:         (11) 99999-0000
   Endereço:         Rua das Flores, 100, Apto 42 - Jardim Primavera, São Paulo/SP - CEP: 01001-000
   Valor Base:       R$ 1.000,00
   Imposto (3%):     R$ 30,00
   Total a Pagar:    R$ 1.030,00

   --- 🏢 Cadastro de Pessoa Jurídica ---
   Nome Fantasia:    ClientLab Tecnologia
   Razão Social:     ClientLab Tecnologia Ltda.
   CNPJ:             11222333000181
   Telefone:         (11) 3333-4444
   Endereço:         Avenida Central, 500, Torre A, Sala 1502 - Centro Empresarial, São Paulo/SP - CEP: 01310-100
   Valor Base:       R$ 10.000,00
   Imposto (5%):     R$ 500,00
   Total a Pagar:    R$ 10.500,00

   --- 🧪 Validações de Regras de Negócio ---
   [Esperado] Validação Idade: O cadastro permite apenas pessoas com idade igual ou superior a 18 anos. (Parameter 'dataNascimento')
   [Esperado] Validação CNPJ:  O CNPJ informado não é válido. (Parameter 'cnpj')
   [Esperado] Validação Valor: O valor para cálculo do imposto deve ser maior que zero. (Parameter 'valor')

   ==========================================================
               Execução concluída com sucesso!               
   ==========================================================
   ```

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem**: C#
- **Plataforma**: .NET 10.0 (Console Application)
- **IDE recomendada**: Visual Studio Code

---

## 👤 Autor

Desenvolvido por **Carlos Niell** durante o curso **SENAI**.
