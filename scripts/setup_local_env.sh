#!/bin/bash

# Description: Sets up environment variables for local development with Terraform and Terragrunt.
# Usage: source scripts/setup_local_env.sh

# Check if the user is logged into Azure
if ! az account show &> /dev/null; then
  echo "Error: You are not logged into Azure. Please run 'az login' first."
  exit 1
fi

# Get the currently selected Azure subscription
SUBSCRIPTION=$(az account show --query id --output tsv | tr -d '\r' | xargs)
SUBSCRIPTION_NAME=$(az account show --query name --output tsv | tr -d '\r' | xargs)

# Get a list of unique tenant IDs
TENANT_IDS=($(az account list --query '[].tenantId' --output tsv | sort -u | tr -d '\r'))

# Check if multiple tenants are found
if [ ${#TENANT_IDS[@]} -gt 1 ]; then
  echo "Multiple tenants found:"
  for i in "${!TENANT_IDS[@]}"; do
    echo "$((i + 1)). ${TENANT_IDS[$i]}"
  done

  # Prompt the user to select one
  read -p "Select a tenant by number: " tenant_choice
  if [[ ! "$tenant_choice" =~ ^[0-9]+$ ]] || [ "$tenant_choice" -lt 1 ] || [ "$tenant_choice" -gt ${#TENANT_IDS[@]} ]; then
    echo "Invalid selection."
    exit 1
  fi

  TENANT_ID="${TENANT_IDS[$((tenant_choice - 1))]}"
else
  TENANT_ID="${TENANT_IDS[0]}"
fi

# Clean up any trailing characters from tenant ID
TENANT_ID=$(echo "$TENANT_ID" | tr -d '\r' | xargs)

# Confirm retrieved values
echo "Using Azure Subscription: $SUBSCRIPTION ($SUBSCRIPTION_NAME)"
echo "Selected Tenant ID: $TENANT_ID"

# Export environment variables for Terraform
export ARM_SUBSCRIPTION_ID="$SUBSCRIPTION"
export ARM_TENANT_ID="$TENANT_ID"

# Display set environment variables
echo "Azure environment variables have been set:"
echo "-----------------------------------------"
echo "ARM_SUBSCRIPTION_ID=$ARM_SUBSCRIPTION_ID"
echo "ARM_TENANT_ID=$ARM_TENANT_ID"
echo "-----------------------------------------"
echo "Local environment setup completed."
