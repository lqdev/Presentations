provider "azurerm" {
  version="~> 1.27"
}

resource "azurerm_resource_group" "iaac-terraform-example" {
    name = "iaac-rg"
    location = "East US"   
}

resource "azurerm_cognitive_account" "iaac-cv-terraform" {
  name="iaac-cv-terraform"
  resource_group_name=azurerm_resource_group.iaac-terraform-example.name
  location=azurerm_resource_group.iaac-terraform-example.location
  kind="ComputerVision"
  sku_name="S1"
}

output "computer_vision_endpoint" {
    value="${azurerm_cognitive_account.iaac-cv-terraform.endpoint}"
}

# Commands:
# terraform init
# terraform plan -out deploymentplan
# terraform apply "deploymentplan"
# az resource list --resource-group iaac-rg --output table