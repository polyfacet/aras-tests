# First run core test
Write-Host -ForegroundColor Cyan "Running Category = Core Tests"
dotnet test --project Aras.Core.Tests\Aras.Core.Tests.csproj --filter-trait "Category=Core" --retry-failed-tests 3 --output Detailed
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }
# Then run the rest
Write-Host -ForegroundColor Cyan "Running Category != Core Tests"
dotnet test --project Aras.Core.Tests\Aras.Core.Tests.csproj --filter-not-trait "Category=Core" --retry-failed-tests 3 --output Detailed
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }