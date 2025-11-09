# Verification script for SubModule API tests
# Run this after restarting your development environment

Write-Host "=== SubModule API Verification Script ===" -ForegroundColor Cyan
Write-Host ""

# Clean shutdown of any running processes
Write-Host "1. Shutting down build servers..." -ForegroundColor Yellow
dotnet build-server shutdown
Start-Sleep -Seconds 2

# Build fresh
Write-Host "`n2. Building test project..." -ForegroundColor Yellow
$buildResult = dotnet build test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj -c Release 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build FAILED!" -ForegroundColor Red
    $buildResult
    exit 1
}
Write-Host "Build succeeded!" -ForegroundColor Green

# Run SubModule tests
Write-Host "`n3. Running SubModule tests..." -ForegroundColor Yellow
dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj `
    --filter "FullyQualifiedName~SubModuleTests" `
    -c Release `
    --verbosity normal `
    --logger "console;verbosity=detailed"

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n=== SUCCESS: All SubModule tests passed! ===" -ForegroundColor Green
} else {
    Write-Host "`n=== FAILURE: Some SubModule tests failed ===" -ForegroundColor Red
    exit 1
}
