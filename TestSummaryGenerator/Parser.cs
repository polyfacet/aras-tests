using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class Parser
{
    public static List<TestMethodInfo> ParseTestMethods(string sourceFolder)
    {
        var testMethods = new List<TestMethodInfo>();
        foreach (var file in Directory.EnumerateFiles(sourceFolder, "*.cs", SearchOption.AllDirectories))
        {
            var code = File.ReadAllText(file);
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var cls in classes)
            {
                var className = cls.Identifier.Text;
                var methods = cls.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var method in methods)
                {
                    var hasFact = method.AttributeLists
                        .SelectMany(a => a.Attributes)
                        .Any(attr => attr.Name.ToString().Contains("Fact") || attr.Name.ToString().Contains("Theory"));
                    if (!hasFact) continue;

                    var traits = new Dictionary<string, string>();
                    foreach (var attr in method.AttributeLists.SelectMany(a => a.Attributes))
                    {
                        if (attr.Name.ToString() == "Trait" && attr.ArgumentList?.Arguments.Count == 2)
                        {
                            var key = attr.ArgumentList.Arguments[0].ToString().Trim('"');
                            var value = attr.ArgumentList.Arguments[1].ToString().Trim('"');
                            traits[key] = value;
                        }
                    }
                    testMethods.Add(new TestMethodInfo(className, method.Identifier.Text, traits));
                }
            }
        }
        return testMethods;
    }
}