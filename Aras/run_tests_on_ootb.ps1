# First run core test
Write-Host -ForegroundColor Cyan "Running Business = OOTB Tests" 
dotnet test Aras.OOTB.Tests --filter "Business=OOTB" --logger "console;verbosity=detailed"
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }
# Then run the rest
Write-Host -ForegroundColor Cyan "Running Smoke tests"
dotnet test Aras.OOTB.Tests --filter "SmokeTest=1" --logger "console;verbosity=detailed"
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }