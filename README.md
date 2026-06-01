# Sistema de Controle - CRUD Windows Forms com Excel

Aplicação desktop Windows Forms (C# / .NET) que utiliza arquivo Excel (.xlsx) como banco de dados via ClosedXML.

## Pré-requisitos

- **.NET 8 SDK** (ou superior) instalado — [Download](https://dotnet.microsoft.com/download)
- Windows (necessário para Windows Forms)

## Como compilar

```bash
dotnet build
```

## Como executar

```bash
dotnet run --project "C:\Users\Everton Ferreira\Documents\crud-windows-app\CrudExcelApp.csproj"
```

Ou simplesmente:

```bash
cd "C:\Users\Everton Ferreira\Documents\crud-windows-app"
dotnet run
```

## Como rodar os testes E2E

```bash
dotnet run --project Tests
```

## Estrutura

| Arquivo | Descrição |
|---------|-----------|
| `CrudExcelApp.csproj` | Projeto principal WinForms |
| `Program.cs` | Ponto de entrada da aplicação |
| `ExcelDatabase.cs` | Camada de dados — CRUD genérico sobre planilhas Excel |
| `FormHome.cs` | Tela inicial com 4 botões de navegação |
| `FormVeiculos.cs` | Cadastro de Entradas de Veículos |
| `FormFornecedores.cs` | Entrada de Fornecedores |
| `FormCaminhoes.cs` | Entrada de Caminhões da Casa |
| `FormChaves.cs` | Registro de Chaves |
| `Tests/` | Projeto de testes E2E (console app) |

## Tela Inicial

A tela inicial exibe o título **"Sistema de Controle"** e 4 botões:

1. **Cadastro de Entradas de Veículos** — Gerencia veículos com Placa, Modelo, Motorista, Data Entrada e Observações.
2. **Entrada de Fornecedores** — Gerencia fornecedores com Nome, CNPJ, Telefone, Email e Data Entrada.
3. **Entrada de Caminhões da Casa** — Gerencia caminhões próprios com Placa, Modelo, Motorista, Data Entrada e Observações.
4. **Registro de Chaves** — Gerencia chaves com Identificação, Responsável, Data Retirada, Data Devolução e Status.

## Módulos

Cada módulo possui:
- Campos de entrada (TextBox / DateTimePicker)
- Botões: **Incluir**, **Editar**, **Apagar**, **Limpar**, **Voltar**
- DataGridView com listagem completa dos registros
- Validação de campo obrigatório (Placa / Nome / Identificação)
- Confirmação antes de apagar
- Mensagens de sucesso/erro via MessageBox

## Banco de Dados

O arquivo `database.xlsx` é criado automaticamente na primeira execução no diretório do executável. Contém 4 planilhas com auto-incremento de ID.
