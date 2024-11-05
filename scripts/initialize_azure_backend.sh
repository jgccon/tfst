#!/bin/bash
# This script initializes the Azure backend resources for Terraform state storage based on the specified environment.

# Set variables for the Azure backend configuration.
SUBSCRIPTION=${1:-"<your-subscription-id>"}  # Replace with your Azure subscription ID or pass as an argument.
ENVIRONMENT_NAME=${2:-"dev"}  # Pass the environment name (dev, prod, etc.) as the second argument.
TFSTATE_RG_NAME="tfst-tfstate-$ENVIRONMENT_NAME"
TFSTATE_LOCATION="westeurope"
TFSTATE_STORAGE_ACCOUNT_NAME="tfsttfstate${ENVIRONMENT_NAME}"  # Ensure this name is unique across Azure
TFSTATE_CONTAINER_NAME="tfstate-container"

# Login to Azure
az login

# Set the Azure subscription
az account set --subscription "$SUBSCRIPTION"

# Create the resource group
az group create --name "$TFSTATE_RG_NAME" --location "$TFSTATE_LOCATION"

# Create the storage account
az storage account create --name "$TFSTATE_STORAGE_ACCOUNT_NAME" \
  --resource-group "$TFSTATE_RG_NAME" \
  --location "$TFSTATE_LOCATION" \
  --sku Standard_LRS

# Create the blob container
az storage container create --name "$TFSTATE_CONTAINER_NAME" \
  --account-name "$TFSTATE_STORAGE_ACCOUNT_NAME"

echo "Azure backend setup complete."
echo "Resource Group: $TFSTATE_RG_NAME"
echo "Storage Account: $TFSTATE_STORAGE_ACCOUNT_NAME"
echo "Container: $TFSTATE_CONTAINER_NAME"
