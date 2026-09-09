# First run core test
Write-Host -ForegroundColor Cyan "Running Business = OOTB Tests" 
dotnet test --project Aras.OOTB.Tests\Aras.OOTB.Tests.csproj --filter-trait "Business=OOTB" --retry-failed-tests 3 --output Detailed
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }
# Then run the rest
Write-Host -ForegroundColor Cyan "Running Smoke tests"
dotnet test --project Aras.OOTB.Tests\Aras.OOTB.Tests.csproj --filter-trait "SmokeTest=1" --retry-failed-tests 3 --output Detailed
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }