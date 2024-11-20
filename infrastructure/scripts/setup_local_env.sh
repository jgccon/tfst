#!/bin/bash

# Description: Sets up environment variables for local development with Terraform and Terragrunt.
# Usage: source scripts/setup_local_env.sh

echo "🔧 Starting Azure local environment setup..."

# 1. Check if the user is already logged into Azure
if ! az account show &> /dev/null; then
  echo "🔑 You are not logged into Azure. Initiating login..."
  az login
  if [ $? -ne 0 ]; then
    echo "❌ Error: Azure authentication failed."
    exit 1
  fi
else
  echo "✅ You are already authenticated in Azure."
fi

# 2. List all available subscriptions and retrieve the IDs and names
echo "🔍 Retrieving list of subscriptions..."
SUBSCRIPTIONS=$(az account list --query '[].{Name:name, Id:id, TenantId:tenantId}' -o json)

# Check if subscriptions were retrieved
if [ -z "$SUBSCRIPTIONS" ]; then
  echo "❌ No subscriptions found. Exiting..."
  exit 1
fi

# Display the list of subscriptions in a user-friendly format
INDEX=0
echo "$SUBSCRIPTIONS" | jq -c '.[]' | while read -r subscription; do
  NAME=$(echo "$subscription" | jq -r '.Name')
  ID=$(echo "$subscription" | jq -r '.Id')
  TENANT=$(echo "$subscription" | jq -r '.TenantId')
  echo "[$INDEX] $NAME ($ID) [Tenant: $TENANT]"
  INDEX=$((INDEX + 1))
done

# Allow the user to select a subscription by index
read -p "🔄 Select the number of the subscription you wish to use (0 for default): " sub_index

# Parse the selected subscription details
SUBSCRIPTION_ID=$(echo "$SUBSCRIPTIONS" | jq -r ".[$sub_index].Id")
SUBSCRIPTION_NAME=$(echo "$SUBSCRIPTIONS" | jq -r ".[$sub_index].Name")
TENANT_ID=$(echo "$SUBSCRIPTIONS" | jq -r ".[$sub_index].TenantId")

# Validate the selected subscription
if [ -z "$SUBSCRIPTION_ID" ] || [ -z "$TENANT_ID" ]; then
  echo "❌ Invalid selection. Exiting..."
  exit 1
fi

# 4. Set the selected subscription
echo "🔄 Setting the selected subscription..."
az account set --subscription "$SUBSCRIPTION_ID"
if [ $? -ne 0 ]; then
  echo "❌ Error: Failed to set the selected subscription."
  exit 1
fi

# Export environment variables for Terraform
export ARM_SUBSCRIPTION_ID="$SUBSCRIPTION_ID"
export ARM_TENANT_ID="$TENANT_ID"

# 6. Confirm the configuration
echo "✅ Configuration completed:"
echo "-----------------------------------------"
echo "🔹 Subscription: $SUBSCRIPTION_NAME ($ARM_SUBSCRIPTION_ID)"
echo "🔹 Tenant ID: $ARM_TENANT_ID"
echo "-----------------------------------------"
echo "🌍 Local environment is now set up for Terraform and Terragrunt."
