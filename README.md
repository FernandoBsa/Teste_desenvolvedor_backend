
# 🛍️ InsideStore

Projeto de WebAPI desenvolvido como parte de um teste técnico para vaga de desenvolvedor backend. A API permite gerenciar pedidos e produtos de uma loja, seguindo boas práticas de arquitetura, SOLID e DDD.

---

## ✅ Funcionalidades implementadas

- ✅ Iniciar um novo pedido  
- ✅ Adicionar produtos a um pedido  
- ✅ Remover produtos de um pedido  
- ✅ Fechar um pedido (somente se houver ao menos um produto)  
- ✅ Listar todos os pedidos  
- ✅ Obter detalhes de um pedido pelo ID  
- ✅ CRUD completo de produtos  

---

## ⚙️ Tecnologias e padrões utilizados

- **.NET 8**
- **Entity Framework Core**
- **AutoMapper**
- **xUnit** + **Moq** (testes unitários)
- **Result Pattern** (padronização de retornos com sucesso e erro)
- **Repository Pattern**
- **Domain-Driven Design (DDD)**
- **Swagger**
- **Handler global de exceções**

---

## 🧱 Estrutura de pastas

```
InsideStore/
│
├── src/
│   ├── InsideStore.Api           → Camada de apresentação (controllers, configs)
│   ├── InsideStore.Application  → Regras de negócio, serviços e interfaces
│   ├── InsideStore.Domain       → Entidades, DTOs, enums e regras de domínio
│   └── InsideStore.Infra        → Persistência, EF, migrations, repositórios
│
├── test/
│   └── InsideStore.Tests        → Testes unitários com xUnit
```

---

## 🧪 Testes

- Os testes unitários foram criados utilizando **xUnit** e **Moq**, cobrindo os principais fluxos de:
  - Criação, edição e exclusão de produtos
  - Adição, remoção e fechamento de pedidos
  - Regras de negócio como:
    - Não permitir alterações em pedidos fechados
    - Impedir fechamento de pedidos sem produtos

---

## ▶️ Como executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/InsideStore.git
   cd InsideStore
   ```

2. Execute as migrations (se estiver usando banco real).
    ```bash
   dotnet ef database update
   ```

3. Execute o projeto:
   ```bash
   dotnet run --project src/InsideStore.Api
   ```
---

## 📦 Banco de dados

- Utilizado **Entity Framework Core** com suporte a:
  - Pode ser configurado para SQL Server, PostgreSQL, etc.

---

## 📄 Requisitos do teste atendidos

✔ WebAPI com rotas para:
- Iniciar, adicionar/remover produtos, fechar e listar pedidos  
✔ Regras de negócio aplicadas (ex: não fechar pedidos vazios)  
✔ Aplicação de padrões de projeto (Repository, Result, DDD)  
✔ Testes unitários com cobertura dos principais cenários  
✔ Estrutura limpa, separada por responsabilidades  

---

## 🚀 Melhorias possíveis

- Autenticação e autorização
- Logs com Serilog
- Cache com Redis
- Testes de integração

---

## 🧑‍💻 Autor

- Desenvolvido por: Fernando Barros
- Entre em contato: fernandobarrosdesak@gmail.com
- LinkedIn: [https://www.linkedin.com/in/fernandobarrosdev/](https://www.linkedin.com/in/fernandobarrosdev/)
