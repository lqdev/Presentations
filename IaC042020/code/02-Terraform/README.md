# Terraform

Steps:

1. `terraform init`
1. `terraform plan -out deploymentplan`
1. `terraform apply "deploymentplan"`
1. `az resource list --resource-group iaac-rg --output table`