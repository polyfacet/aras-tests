string sourceFolder = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();
string primaryTraitKey = args.Length > 1 ? args[1] : "Domain";
DirectoryInfo di = new DirectoryInfo(sourceFolder);
Console.WriteLine($"Scanning folder: {di.FullName}");

var testMethods = Parser.ParseTestMethods(sourceFolder);
ReportGenerator.WriteTestSummaryReport(sourceFolder, testMethods, primaryTraitKey, includeClassName: false);
