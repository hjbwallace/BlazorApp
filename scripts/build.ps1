$srcPath = Join-Path -Path $PSScriptRoot -ChildPath '..\src'
$artifactsPath = Join-Path -Path $srcPath -ChildPath '..\artifacts'

# Remove any previous artifacts 
if (Test-path $artifactsPath) { Remove-Item -Recurse -Force $artifactsPath }
New-Item -Path $artifactsPath -ItemType Directory | Out-Null

$build_version = "1.0.0"

# Build the projects that will not be published
dotnet clean "$($srcPath)\BlazorApp.sln"
dotnet restore "$($srcPath)\BlazorApp.sln"

dotnet build "$($srcPath)\BlazorApp\BlazorApp.csproj"
dotnet build "$($srcPath)\BlazorApp.Client\BlazorApp.Client.csproj"
dotnet build "$($srcPath)\BlazorApp.Server.Tests\BlazorApp.Server.Tests.csproj"
dotnet build "$($srcPath)\BlazorApp.Tests\BlazorApp.Tests.csproj"

# Publish the Server project and save the artifacts path
dotnet publish "$($srcPath)\BlazorApp.Server\BlazorApp.Server.csproj" -c Release --no-restore /p:Version=$build_version

Compress-Archive -Path "$($srcPath)\BlazorApp.Server\bin\Release\netcoreapp3.1\publish\*" -DestinationPath "$($artifactsPath)\BlazorApp.Server.package.zip" -CompressionLevel Optimal -Force