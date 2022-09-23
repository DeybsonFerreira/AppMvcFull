CLI
dotnet projectExample.dll --environment=Development
dotnet run --project "projectExample.csproj"

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

# Fluent Validations
> Business > dotnet add package FluentValidation --version 11.2.2



# DEPLOY
> Self-Contained > Entregar o framework no servidor junto com a aplicação do deploy.
> Framework-Dependent > Quando servidor já tem o framework instalado 


 Para setar a variável de ambiente no deploy (do IIS) basta utilizar o código no arquivo deploy.pubxml

<environmentVariables>
  <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
 </environmentVariables>




/************ARQUIVO BAT**************************/
@echo iniciando
@echo *****************************************
cd src\appmvcfull.app\
dotnet restore
dotnet run --project appmvcfull.app.csproj

pause

