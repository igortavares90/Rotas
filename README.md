# Rota de Viagem - API .NET 9.0

## 1. Banco de Dados

- Execute o script `script-criacao-banco.sql` no SQL Server 2022 Express
- A string de conexão está configurada em `appsettings.json`

Exemplo:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=RotaViagemDb;User Id=sa;Password=Teste@2025;"
}