#!/bin/bash

# Descripción: Inicializa los recursos de backend en Azure para Terraform.
# Uso: bash scripts/initialize_azure_backend.sh <subscription-id> <environment>

# Variables
SUBSCRIPTION=${1:-""}
ENVIRONMENT_NAME=${2:-"dev"}

# Parameter validation
if [ -z "$SUBSCRIPTION" ]; then
  echo "Error: Debes proporcionar el ID de suscripción como primer parámetro."
  exit 1
fi

# Configuration variables
TFSTATE_RG_NAME="tfst-tfstate-$ENVIRONMENT_NAME"
TFSTATE_LOCATION="westeurope"
TFSTATE_STORAGE_ACCOUNT_NAME="tfsttfstate${ENVIRONMENT_NAME}"
TFSTATE_CONTAINER_NAME="tfstate-container"

echo "Initializing Azure backend..."
az login

echo "Stablishing subscription: $SUBSCRIPTION..."
az account set --subscription "$SUBSCRIPTION"
export ARM_SUBSCRIPTION_ID=$SUBSCRIPTION

echo "Creanting resource group: $TFSTATE_RG_NAME..."
az group create --name "$TFSTATE_RG_NAME" --location "$TFSTATE_LOCATION"

echo "Creating storage account: $TFSTATE_STORAGE_ACCOUNT_NAME..."
az storage account create --name "$TFSTATE_STORAGE_ACCOUNT_NAME" \
  --resource-group "$TFSTATE_RG_NAME" \
  --location "$TFSTATE_LOCATION" \
  --sku Standard_LRS

echo "Creating storage container: $TFSTATE_CONTAINER_NAME..."
az storage container create --name "$TFSTATE_CONTAINER_NAME" \
  --account-name "$TFSTATE_STORAGE_ACCOUNT_NAME"

echo "Backend initialization completed."
echo "======================================"
echo "Resource Group: $TFSTATE_RG_NAME"
echo "Storage Account: $TFSTATE_STORAGE_ACCOUNT_NAME"
echo "Container: $TFSTATE_CONTAINER_NAME"
echo "======================================"
