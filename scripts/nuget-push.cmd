dotnet pack ../src/BlazingDev.BlazingExtensions/BlazingDev.BlazingExtensions.csproj --output .
set /p Version=Enter version number:
set /p Apikey=Enter API-Key:
dotnet nuget push BlazingDev.BlazingExtensions.%Version%.nupkg --api-key %Apikey% --source https://api.nuget.org/v3/index.json
pause