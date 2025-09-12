public class ReportGenerator
{
    public static void WriteTestSummaryReport(string sourceFolder, List<TestMethodInfo> testMethods, string primaryTraitKey, bool includeClassName)
    {
        var traitGroups = new Dictionary<string, List<(string ClassName, string MethodName, string TraitValue)>>();

        foreach (var test in testMethods)
        {
            if (test.Traits.Count == 0)
            {
                if (!traitGroups.ContainsKey("No Trait"))
                    traitGroups["No Trait"] = new List<(string, string, string)>();
                traitGroups["No Trait"].Add((test.ClassName, test.MethodName, ""));
            }
            else
            {
                foreach (var trait in test.Traits)
                {
                    var key = $"{trait.Key}: {trait.Value}";
                    if (!traitGroups.ContainsKey(key))
                        traitGroups[key] = new List<(string, string, string)>();
                    traitGroups[key].Add((test.ClassName, test.MethodName, trait.Value));
                }
            }
        }

        var md = new System.Text.StringBuilder();
        md.AppendLine("# Test Summary\n");

        // List "primaryTraitKey" traits first
        foreach (var group in traitGroups.Where(g => g.Key.StartsWith($"{primaryTraitKey}:")))
        {
            md.AppendLine($"## {group.Key}").AppendLine();
            foreach (var test in group.Value)
            {
                if (includeClassName)
                    md.AppendLine($"- `{test.ClassName}.{test.MethodName}`");
                else
                    md.AppendLine($"- `{test.MethodName}`");
            }
            md.AppendLine();
        }

        // Then list all other traits except "primaryTraitKey"
        foreach (var group in traitGroups.Where(g => !g.Key.StartsWith($"{primaryTraitKey}:")))
        {
            md.AppendLine($"## {group.Key}").AppendLine();
            foreach (var test in group.Value)
            {
                if (includeClassName)
                    md.AppendLine($"- `{test.ClassName}.{test.MethodName}`");
                else
                    md.AppendLine($"- `{test.MethodName}`");
            }
            md.AppendLine();
        }
        string fullPath = Path.GetFullPath(Path.Combine(sourceFolder, "TestSummary.md"));
        File.WriteAllText(fullPath, md.ToString());
        Console.WriteLine($"Test summary generated: {fullPath}");
    }
}