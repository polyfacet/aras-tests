using System.Text;

public record TraitGroupEntry(string ClassName, string MethodName, string TraitValue);

public class ReportGenerator
{
    public static void WriteTestSummaryReport(string sourceFolder, List<TestMethodInfo> testMethods, string primaryTraitKey, bool includeClassName)
    {
        var traitGroups = LoadTraitGroups(testMethods);

        var md = new System.Text.StringBuilder();
        md.AppendLine("# Test Summary\n");

        // Sort group keys: primaryTraitKey first, then alphabetical, "No Trait" last
        var sortedGroupKeys = traitGroups.Keys
            .OrderBy(k =>
                k == "No Trait" ? 2
                : k.StartsWith($"{primaryTraitKey}:") ? 0
                : 1)
            .ThenBy(k => k == "No Trait" ? "" : k)
            .ToList();

        AppendTableOfContents(md, sortedGroupKeys);

        foreach (var key in sortedGroupKeys)
        {
            var group = new KeyValuePair<string, List<TraitGroupEntry>>(key, traitGroups[key]);
            AppendGroup(md, group, includeClassName);
        }

        string fullPath = Path.GetFullPath(Path.Combine(sourceFolder, "TestSummary.md"));
        File.WriteAllText(fullPath, md.ToString());
        Console.WriteLine($"Test summary generated: {fullPath}");
    }

    private static Dictionary<string, List<TraitGroupEntry>> LoadTraitGroups(List<TestMethodInfo> testMethods)
    {
        var traitGroups = new Dictionary<string, List<TraitGroupEntry>>();
        foreach (var test in testMethods)
        {
            if (test.Traits.Count == 0)
            {
                if (!traitGroups.ContainsKey("No Trait"))
                    traitGroups["No Trait"] = new List<TraitGroupEntry>();
                traitGroups["No Trait"].Add(new TraitGroupEntry(test.ClassName, test.MethodName, ""));
            }
            else
            {
                foreach (var trait in test.Traits)
                {
                    var key = $"{trait.Key}: {trait.Value}";
                    if (!traitGroups.ContainsKey(key))
                        traitGroups[key] = new List<TraitGroupEntry>();
                    traitGroups[key].Add(new TraitGroupEntry(test.ClassName, test.MethodName, trait.Value));
                }
            }
        }
        return traitGroups;
    }

    private static void AppendGroup(StringBuilder md, KeyValuePair<string, List<TraitGroupEntry>> group, bool includeClassName)
    {
        md.AppendLine($"## {group.Key}").AppendLine();
        foreach (var test in group.Value.OrderBy(t => t.MethodName))
        {
            if (includeClassName)
                md.AppendLine($"- `{test.ClassName}.{test.MethodName}`");
            else
                md.AppendLine($"- `{test.MethodName}`");
        }
        md.AppendLine();
    }

    private static void AppendTableOfContents(StringBuilder md, List<string> allGroupKeys)
    {
        // Table of Contents
        md.AppendLine("**Table of Contents**").AppendLine();
        foreach (var key in allGroupKeys)
        {
            // Markdown anchor: replace spaces and punctuation with '-', lower-case
            var anchor = key.Replace(" ", "-").Replace(":", "").Replace(".", "").ToLower();
            md.AppendLine($"- [{key}](#{anchor})");
        }
        md.AppendLine();
    }
}