# First run core test
Write-Host -ForegroundColor Cyan "Running Category = Core Tests"
dotnet test ArasTests --filter "Category=Core" -v q
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }
# Then run the rest
Write-Host -ForegroundColor Cyan "Running Category != Core Tests"
dotnet test ArasTests --filter "Category!=Core" -v q
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }