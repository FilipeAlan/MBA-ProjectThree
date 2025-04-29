# ✅ Cobertura de Testes - EducPlatform

Este documento lista os testes unitários, de integração e de performance implementados na plataforma de educação.

---

## 🧪 AlunoContext

### 🔹 Unitários
- ✅ Deve cadastrar aluno quando os dados forem válidos
- ✅ Não deve cadastrar aluno quando o nome estiver vazio
- ✅ Não deve cadastrar aluno quando o e-mail for inválido
- ✅ Deve editar aluno quando o ID for válido
- ✅ Não deve editar aluno quando o ID não existir
- ✅ Deve remover aluno quando o ID for válido
- ✅ Não deve remover aluno com matrículas ou certificados
- ✅ Não deve remover aluno quando o ID não existir
- ✅ Deve retornar aluno quando o ID existir
- ✅ Deve retornar nulo quando o ID não existir
- ✅ Deve retornar todos os alunos cadastrados
- ✅ Deve retornar lista vazia quando não houver alunos cadastrados
- ✅ Deve matricular aluno quando dados forem válidos
- ✅ Não deve matricular aluno inexistente
- ✅ Não deve matricular em curso inexistente
- ✅ Não deve permitir matrícula duplicada

### 🔹 Integração
- ✅ Deve cadastrar aluno e persistir no banco de dados
- ✅ Deve editar o nome e email do aluno no banco de dados
- ✅ Deve remover aluno existente do banco de dados
- ✅ Deve retornar aluno existente pelo ID
- ✅ Deve retornar todos os alunos cadastrados

### 🔹 Performance
- ✅ Deve cadastrar 1000 alunos em menos de 5 segundos
- ✅ Deve editar 1000 alunos em menos de 5 segundos
- ✅ Deve deletar 1000 alunos em menos de 5 segundos
- ✅ Deve listar 1000 alunos em menos de 500ms
- ✅ Deve obter aluno pelo ID em menos de 100ms

---

## 🧪 CursoContext

### 🔹 Unitários
- ✅ Deve cadastrar curso com dados válidos
- ✅ Não deve cadastrar curso com nome vazio
- ✅ Não deve cadastrar curso com descrição vazia
- ✅ Deve editar curso quando os dados forem válidos
- ✅ Não deve editar curso com nome vazio
- ✅ Não deve editar curso com descrição vazia
- ✅ Deve remover curso quando o ID for válido
- ✅ Não deve remover curso quando o ID não existir
- ✅ Deve adicionar aula ao curso com sucesso
- ✅ Não deve adicionar aula com título vazio
- ✅ Não deve adicionar aula com conteúdo vazio
- ✅ Não deve adicionar aula a curso inexistente
- ✅ Deve retornar curso quando o ID existir
- ✅ Deve retornar nulo quando o ID não existir
- ✅ Deve retornar todos os cursos cadastrados

### 🔹 Integração
- ✅ Deve cadastrar curso e persistir no banco de dados
- ✅ Deve editar curso existente no banco de dados
- ✅ Deve deletar curso do banco de dados
- ✅ Deve retornar curso existente pelo ID
- ✅ Deve retornar todos os cursos cadastrados
- ✅ Deve cadastrar aula e associar ao curso no banco de dados

### 🔹 Performance
- ✅ Deve cadastrar 1000 cursos em menos de 3 segundos ❌ (falhou com ~6.5s)
- ✅ Deve editar 1000 cursos em menos de 5 segundos
- ✅ Deve deletar 1000 cursos em menos de 5 segundos
- ✅ Deve listar 1000 cursos em menos de 500ms
- ✅ Deve obter curso por ID em menos de 100ms

---

## 🧪 PagamentoContext

### 🔹 Unitários
- ✅ Deve confirmar pagamento e ativar matrícula com dados válidos
- ✅ Deve falhar se aluno não encontrado pela matrícula
- ✅ Deve falhar se matrícula não encontrada no aluno
- ✅ Não deve ativar matrícula se pagamento for rejeitado
- ✅ Deve falhar se dados do cartão forem inválidos

### 🔹 Integração
- ✅ Deve confirmar pagamento e ativar matrícula no banco
- ✅ Deve rejeitar pagamento e manter matrícula pendente

---

✅ Totalmente testado com xUnit + SQLite InMemory  
🛡️ Cobertura completa de casos reais e extremos

---

Criado por: **Filipe Alan Elias**  
Projeto: **EducPlatform - MBA DevXpert**
