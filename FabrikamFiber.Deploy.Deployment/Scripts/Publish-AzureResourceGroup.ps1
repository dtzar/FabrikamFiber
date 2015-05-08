#Requires -Version 3.0

<#
This is a preview release.
What else would you like this script to do for you?  Send us feedback here: http://go.microsoft.com/fwlink/?LinkID=399398 
#>

Set-StrictMode -Version 3

Switch-AzureMode AzureResourceManager

# Create or update the resource group using our template file and template parameters
New-AzureResourceGroup -Name 'FabrikamFiber' -Location "West US" -TemplateFile '..\Templates\WebSiteSQLDatabase.json' -TemplateParameterFile '..\Templates\WebSiteSQLDatabase.param.dev.json' -Force -Verbose
