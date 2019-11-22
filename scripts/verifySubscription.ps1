Param(
    [ValidateNotNullOrEmpty()]
    [string] $subscriptionId,

    [string] $webappName
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version "Latest"

Write-Host 'The name of the webapp'
Write-Host $webappName

# Sets the Azure subscription
az account set --subscription $subscriptionId

Write-Host 'Success'