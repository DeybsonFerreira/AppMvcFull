# Template folder
/App
/Data
/Business

# Install AutoMapper
/App > dotnet add package AutoMapper --version 11.0.1
/App > dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 11.0.0

# Install Entity Framework CLI
/Data > dotnet add package Microsoft.EntityFrameworkCore --version 6.0.8
/Data > dotnet add package Microsoft.EntityFrameworkCore.Relational --version 6.0.8
/Data > dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.8

# Using Migrations

add-migration InitialTables -Context AppMvcFullDbContext -o Migrations_AppMvcFull >> para criar o migration initial
Script-Migration -Context AppMvcFullDbContext >> para gerar o script
Update-Database -Context AppMvcFullDbContext  >> Para gerar o migration no banco
Remove-Migration -Context AppMvcFullDbContext >> remover migration