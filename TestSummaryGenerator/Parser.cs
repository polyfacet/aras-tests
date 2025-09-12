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

            // Build a map of const string values in the file
            var constsMap = BuildConstMap(root);
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
                            var keyExpr = attr.ArgumentList.Arguments[0].Expression;
                            var valueExpr = attr.ArgumentList.Arguments[1].Expression;

                            string key = keyExpr.ToString().Trim('"');
                            string value;
                            // If value is an identifier and matches a const, use the const value
                            if (valueExpr is IdentifierNameSyntax id && constsMap.TryGetValue(id.Identifier.Text, out var constValue))
                                value = constValue;
                            else
                                value = valueExpr.ToString().Trim('"');

                            traits[key] = value;
                        }
                    }
                    testMethods.Add(new TestMethodInfo(className, method.Identifier.Text, traits));
                }
            }
        }
        return testMethods;
    }

    private static Dictionary<string, string> BuildConstMap(SyntaxNode root)
    {
        return root.DescendantNodes()
            .OfType<FieldDeclarationSyntax>()
            .Where(f => f.Modifiers.Any(m => m.Text == "const"))
            .SelectMany(f => f.Declaration.Variables
                .Where(v => f.Declaration.Type.ToString() == "string" && v.Initializer != null)
                .Select(v => new
                {
                    Name = v.Identifier.Text,
                    Value = v.Initializer.Value.ToString().Trim('"')
                }))
            .ToDictionary(x => x.Name, x => x.Value);
    }
}