@echo iniciando
@echo *****************************************
cd src\appmvcfull.app\
dotnet restore
dotnet run --project appmvcfull.app.csproj

pause
