Param(
    [ValidateNotNullOrEmpty()]
    [string] $subscriptionId,

    [ValidateNotNullOrEmpty()]
    [string] $webappName
)

# Warnings are treated as a script error which causes the step to fail
# Github Actions automatically prepends $ErrorActionPreference = 'stop' to scripts
$ErrorActionPreference = 'SilentlyContinue'

$resourceGroupName = "$($webappName)RG"
$webappZip = Join-Path -Path $PSScriptRoot -ChildPath "..\artifacts\BlazorApp.Server.package.zip"

# Sets the Azure subscription
az account set --subscription $subscriptionId

# Deploy the server webapp
Write-Host 'Stopping the current webapp'
az webapp stop --resource-group $resourceGroupName --name $webappName 

Write-Host 'Deploying the server webapp via zip file'
az webapp deployment source config-zip --resource-group $resourceGroupName --name $webappName --src $webappZip 

Write-Host 'Starting the webapp'
az webapp start --resource-group $resourceGroupName --name $webappName 