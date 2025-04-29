# MBA-ProjectThree - Plataforma de Educação e Treinamento Online

## 1. Apresentação
Este projeto é uma plataforma de educação online desenvolvida como parte do Terceiro Módulo do MBA em Desenvolvimento de Software.

A plataforma é composta por três contextos principais:
- Gerenciamento de Alunos, Matrículas e Certificados
- Gerenciamento de Cursos e Aulas
- Processamento de Pagamentos

## 2. Proposta do Projeto
Permitir o gerenciamento completo de uma plataforma de ensino, com foco em:
- Cadastro de Alunos
- Criação de Cursos e Aulas
- Matrícula de Alunos em Cursos
- Realização e Confirmação de Pagamentos
- Emissão de Certificados

## 3. Tecnologias Utilizadas
- **C# 11**
- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQLite** (banco de dados em memória para testes)
- **XUnit** (testes unitários e de integração)
- **Swagger** (documentação de APIs)

## 4. Estrutura do Projeto

```
/src
  /AlunoContext
    /Domain
    /Application
    /Infrastructure
  /CursoContext
    /Domain
    /Application
    /Infrastructure
  /PagamentoContext
    /Domain
    /Application
    /Infrastructure
  /BuildingBlocks
/tests
  /AlunoContext.Tests
  /CursoContext.Tests
  /PagamentoContext.Tests
```

- **AlunoContext**: Gerenciamento de alunos, matrículas e certificados.
- **CursoContext**: Gerenciamento de cursos e aulas.
- **PagamentoContext**: Controle de pagamentos relacionados a matrículas.
- **BuildingBlocks**: Componentes genéricos reutilizáveis como `EntityBase`, `Result`, `UnitOfWork`.

## 5. Funcionalidades Implementadas
- Cadastro de Alunos
- Cadastro de Cursos
- Criação de Aulas para Cursos
- Matrícula de Alunos em Cursos
- Realização de Pagamento de Matrículas
- Ativação automática da Matrícula após pagamento confirmado
- Emissão de Certificados
- Testes Unitários e de Integração

## 6. Como Executar o Projeto

1. Clone o repositório:
```bash
git clone https://github.com/SeuUsuario/MBA-ProjectThree.git
```

2. Acesse a pasta:
```bash
cd MBA-ProjectThree
```

3. Restaure os pacotes:
```bash
dotnet restore
```

4. Execute a API:
```bash
dotnet run --project src/SeuProjeto.Api
```

5. Acesse a documentação via navegador:
```
https://localhost:5001/swagger
```

## 7. Instruções de Configuração
- A conexão com o banco SQLite é automática (em memória para testes).
- Não é necessária nenhuma instalação ou configuração adicional de banco.

## 8. Documentação da API
- O Swagger está configurado automaticamente ao executar a API.
- URL padrão: `https://localhost:5001/swagger`

## 9. Autor
- Filipe Alan Elias

