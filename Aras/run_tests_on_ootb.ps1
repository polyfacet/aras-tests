# First run core test
Write-Host -ForegroundColor Cyan "Running Business = OOTB Tests"
dotnet test ArasTests --filter "Business=OOTB" -v q
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }
# Then run the rest
Write-Host -ForegroundColor Cyan "Running Smoke tests"
dotnet test ArasTests --filter "SmokeTest=1" -v q
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }