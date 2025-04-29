
# EducPlatform

Projeto desenvolvido para o terceiro módulo do MBA DevXpert.

---

## 📚 Contextos da Solução

O sistema é organizado em **Bounded Contexts** separados:

- **AlunoContext**
  - Gerenciamento de Alunos, Matrículas e Certificados.
- **CursoContext**
  - Gerenciamento de Cursos e Aulas.
- **PagamentoContext**
  - Processamento de Pagamentos de Matrículas.

Cada Contexto possui sua própria camada de **Application**, **Domain**, **Infrastructure** e **Tests**.

---

## 🛠️ Tecnologias e Ferramentas

- .NET 9
- Entity Framework Core (SQLite InMemory para testes)
- xUnit para testes unitários, de integração e performance
- FluentAssertions (opcional nos testes de asserção fluente)
- CQRS (Command Query Responsibility Segregation)
- DDD (Domain-Driven Design)
- Unit of Work (controle de transações)
- Repository Pattern

---

## 🏛️ Organização dos Projetos

```
/src
  /EducPlatform
    /AlunoContext
      /Application
      /Domain
        /Aggregates
        /Entities
        /Enums
        /ValueObjects
      /Infrastructure
        /Context
        /Repositories
      /Tests
        /Integration
        /Performance
        /Shared
        /Unit
    /CursoContext
      (mesma estrutura de camadas)
    /PagamentoContext
      (mesma estrutura de camadas)
```

- **Aggregates**: Raízes de agregados (ex.: `Aluno`, `Curso`, `Pagamento`).
- **Entities**: Entidades que não são raiz (ex.: `Matricula`, `Certificado`, `Aula`).
- **ValueObjects**: Objetos de valor (ex.: `DadosCartao`).
- **Enums**: Tipos enumerados de domínio (ex.: `StatusMatricula`, `StatusPagamento`).

---

## 🔥 Principais Funcionalidades

### AlunoContext
- Cadastro, edição, remoção e consulta de alunos.
- Matrícula de aluno em curso.
- Ativação automática da matrícula após pagamento aprovado.
- Geração de certificados.

### CursoContext
- Cadastro, edição, remoção e consulta de cursos.
- Adição de aulas ao curso.

### PagamentoContext
- Registro de pagamentos de matrícula.
- Simulação de pagamento:
  - Cartões terminados em 0-4: pagamento aprovado.
  - Cartões terminados em 5-9: pagamento rejeitado.
- Atualização automática da matrícula para ativa se pagamento confirmado.

---

## 🧪 Testes Implementados

- **Unit Tests**:
  - Commands Handlers
  - Validações de regras de negócio
- **Integration Tests**:
  - Uso real de banco SQLite em memória.
  - Teste real de repositórios e contexto.
- **Performance Tests**:
  - Tempo de cadastro, edição, listagem e remoção de 1000 registros.

Documentação detalhada dos testes está disponível no arquivo [TESTES.md](./TESTES.md).

---

## ⚡ Observações Importantes

- A ativação de matrícula foi movida para dentro do fluxo de pagamento, integrando dois contextos distintos de maneira direta (injeção de `IAlunoRepository` no `RealizarPagamentoHandler`).
- Melhorias futuras sugeridas:
  - Usar Event-Driven (Mensageria, EventHandler) entre Contextos para reduzir acoplamento.
  - Implementar autenticação e autorização em APIs públicas.
  - Adicionar camada de cache em consultas pesadas.

---

## 🚀 6. Como Executar o Projeto

Clone o repositório:
```
git clone https://github.com/SeuUsuario/MBA-ProjectThree.git
```

Acesse a pasta:
```
cd MBA-ProjectThree
```

Restaure os pacotes:
```
dotnet restore
```

Execute a API:
```
dotnet run --project src/SeuProjeto.Api
```

Acesse a documentação via navegador:
```
https://localhost:5001/swagger
```

---

## ⚙️ 7. Instruções de Configuração

- A conexão com o banco SQLite é automática (banco em memória para testes).
- Não é necessária nenhuma instalação ou configuração adicional de banco de dados.

---

## 📖 8. Documentação da API

- O Swagger está configurado automaticamente ao executar a API.
- URL padrão para acesso:  
  ```
  https://localhost:5001/swagger
  ```

---

## 👨‍💻 Autor

Projeto desenvolvido por:  
**Filipe Alan Elias**  
MBA DevXpert

---
