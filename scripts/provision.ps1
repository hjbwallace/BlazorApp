Param (
    [ValidateNotNullOrEmpty()]
    [string] 
    $subscriptionId,
    
    [ValidateNotNullOrEmpty()]
    [string]
    $webappName,
    
    [ValidateNotNullOrEmpty()]
    [string] $location = "SoutheastAsia"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version "Latest"

try {
    $resourceGroup = "$($webappName)RG"
    $armTemplate = "arm-template.json"
    $deploymentName = [datetime]::UtcNow.Ticks;

    # Sets the Azure subscription
    az account set --subscription $subscriptionId

    # Exit early if the resource group already exists
    if ((az group exists --name $resourceGroup) -eq 'true') { 
        Write-Error 'Resource group already exists'
        exit 1 
    }
    
    # Create the resource group 
    Write-Host 'Creating the resource group ' + $resourceGroup
    az group create --name $resourceGroup --location $location

    # Validate the deployment from the ARM Template
    Write-Host 'Validating the Azure deployment'
    $validationResult = az group deployment validate --resource-group $resourceGroup --template-file $armTemplate --parameters name=$webappName
    $validationResult
    
    if ($null -ne ($validationResult | ConvertFrom-Json).error) { 
        Write-Error 'ARM template is invalid'
        exit 1 
    }

    # Create the deployment
    Write-Host 'Creating the deployment'
    az group deployment create --name $deploymentName --resource-group $resourceGroup --template-file $armTemplate --parameters name=$webappName
}
catch {
    Write-Error $_ -ErrorAction Continue
    exit 1
}