
param(
    [string]$migrationName,
    [string]$projectPath,
    [string]$startupProjectPath,
    [string]$dbContextName
)

# Navigate to the project directory
cd $PSScriptRoot

# Add migration
dotnet ef migrations add $migrationName --project $projectPath --startup-project $startupProjectPath --context $dbContextName

# Update database
dotnet ef database update --project $projectPath --startup-project $startupProjectPath --context $dbContextName
