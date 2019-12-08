$ErrorActionPreference = "Stop"
Set-StrictMode -Version "Latest"

$srcPath = Join-Path -Path $PSScriptRoot -ChildPath '..\src'
dotnet test "$($srcPath)\BlazorApp.Tests\BlazorApp.Tests.csproj" --no-build --no-restore -v quiet
dotnet test "$($srcPath)\BlazorApp.Server.Tests\BlazorApp.Server.Tests.csproj" --no-build --no-restore -v quiet