$testName = "Load_on_find_Parts";

for ($i=1; $i -le 20; $i++) {
    Write-Host "Executing iteration $i" -ForegroundColor Cyan
    # Replace this with your actual command
    dotnet test Aras.OOTB.Tests --filter DisplayName=$testName --logger "console;verbosity=detailed"
    Write-Host "Completed iteration $i" -ForegroundColor Cyan
    if ($i -lt 20) { Start-Sleep -Seconds 60 } # Wait 10 minutes (600 seconds)   
}

Write-Host -ForegroundColor Cyan "Running test $testName"
dotnet test Aras.OOTB.Tests --filter DisplayName=$testName --logger "console;verbosity=detailed"