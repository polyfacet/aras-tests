# First run core test
dotnet test ArasTests --filter "Category=Core" -v q
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }
# Then run the rest
dotnet test ArasTests --filter "Category!=Core" -v q
if ($LASTEXITCODE -gt 0) { return $LASTEXITCODE }