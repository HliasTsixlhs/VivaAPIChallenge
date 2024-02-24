
# PowerShell script to down, rebuild, and up the Docker Compose application

# Navigate to the directory containing the docker-compose.yml file
# Assuming this script is placed in the same directory as the docker-compose.yml
$composeFilePath = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Change directory to where the docker-compose.yml file is located
cd $composeFilePath

# Stop and remove the containers, networks, and volumes
docker-compose down

# Rebuild and start the services
docker-compose up --build -d
