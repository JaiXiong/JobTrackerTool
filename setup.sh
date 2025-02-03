#!/bin/bash

# Generate random suffix for unique names
SUFFIX=$(openssl rand -hex 5)
STATIC_APP_NAME="jobtracker-static-$SUFFIX"
API_APP_NAME="jobtracker-api-$SUFFIX"
RESOURCE_GROUP="JobTracker-RG"
LOCATION="centralus"

echo "Creating Resource Group..."
az group create --name $RESOURCE_GROUP --location $LOCATION

echo "Creating Static Web App..."
az staticwebapp create \
  --name $STATIC_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --location $LOCATION \
  --source https://github.com/JaiXiong/JobTrackerTool \
  --branch main \
  --app-location "/JobTrackerApp" \
  --api-location "/JobTrackerAPI" \
  --output-location "dist/JobTrackerApp"

echo "Creating API App Service..."
az appservice plan create \
  --name "jobtracker-plan" \
  --resource-group $RESOURCE_GROUP \
  --sku F1 \
  --is-linux

az webapp create \
  --name $API_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --plan "jobtracker-plan" \
  --runtime "DOTNET|6.0"

# Configure API URL in Static Web App
az staticwebapp appsettings set \
  --name $STATIC_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --setting-names "API_URL=https://$API_APP_NAME.azurewebsites.net"

echo "==== Deployment Complete ===="
echo "Static Web App URL: https://$STATIC_APP_NAME.azurestaticapps.net"
echo "API URL: https://$API_APP_NAME.azurewebsites.net"

# Save URLs to a file
echo "https://$STATIC_APP_NAME.azurestaticapps.net" > deployment-urls.txt
echo "https://$API_APP_NAME.azurewebsites.net" >> deployment-urls.txt
