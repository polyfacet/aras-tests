# TestSummaryGenerator

TestSummaryGenerator is a console application that scans the source code of an xUnit test project, extracts test methods and their traits, and generates a Markdown summary grouping tests by trait.

## Features

- Parses C# source files for xUnit `[Fact]` and `[Theory]` test methods.
- Extracts `[Trait]` attributes from test methods.
- Groups tests by trait (e.g., Category, Domain).
- Outputs a Markdown report listing tests grouped by trait, with a configurable primary trait key.

## Usage

1. **Build the application**  
   Make sure you have the required NuGet packages (Roslyn).

2. **Run the application**  

   ``` powershell
   dotnet run -- [source-folder] [primaryTraitKey]
   ```

   - `[source-folder]` (optional): The folder to scan for test files. If not specified, the current directory is used.
   - `[primaryTraitKey]` (optional): The trait key to group first in the report (e.g., `Category`, `Domain`). If not specified, defaults to `Domain`.

3. **View the report**  
   - The generated summary is saved as `TestSummary.md` in the source folder.

## Example Output

```markdown
# Test Summary

## Domain: Documents

- `Admin_can_create_Document`
- `Admin_can_NOT_create_Document_without_ItemNumber`

## Domain: ECO

- `Admin_can_create_ECO`
- `Admin_can_NOT_create_ECO_without_a_Title`
- `CM_can_Release_an_Item_via_ECO`

## Domain: Part

- `Admin_can_create_Part`
- `Users_can_manually_Release_Part`

## Category: Core

- `LoginWithFixture_ShouldHaveALoggedInUser`

## No Trait

- `TestLab`
```

## Project Structure

- `Program.cs` – Entry point, coordinates parsing and report generation.
- `Parser.cs` – Contains logic to parse test methods and traits.
- `ReportGenerator.cs` – Generates the Markdown summary report.

## Customization

- Specify the primary trait key as a command-line argument to control which trait is grouped first in the report.

## Requirements

- .NET 6 or later
- Microsoft.CodeAnalysis.CSharp NuGet package

## License

MIT (or specify your license)
