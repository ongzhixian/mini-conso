# dotnet CLI

## Install packages

dotnet add .\Conso\ package Microsoft.Extensions.Hosting
dotnet add .\Conso\ package Microsoft.Extensions.Configuration.UserSecrets
dotnet add .\Conso\ package Microsoft.Extensions.Http


## Add secrets

dotnet user-secrets init --project .\Conso\

dotnet user-secrets --project .\Conso\ set sample "sample secret"