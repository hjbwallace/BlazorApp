Param(
    [ValidateNotNullOrEmpty()]
    [string] $subscriptionId,

    [ValidateNotNullOrEmpty()]
    [string] $webappName
)

$resourceGroupName = "$($webappName)RG"
$webappZip = Join-Path -Path $PSScriptRoot -ChildPath "..\artifacts\BlazorApp.Server.package.zip"

# Sets the Azure subscription
az account set --subscription $subscriptionId

# Deploy the server webapp
Write-Host 'Deploying the server webapp'
az webapp stop --resource-group $resourceGroupName --name $webappName 
az webapp deployment source config-zip --resource-group $resourceGroupName --name $webappName --src $webappZip 
az webapp start --resource-group $resourceGroupName --name $webappName 