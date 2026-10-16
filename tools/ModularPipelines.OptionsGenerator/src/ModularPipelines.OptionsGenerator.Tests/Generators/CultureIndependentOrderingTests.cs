using System.Globalization;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class CultureIndependentOrderingTests
{
    [Test]
    [Arguments("")]
    [Arguments("en-US")]
    [Arguments("sv-SE")]
    [Arguments("tr-TR")]
    public async Task Generated_Members_Use_Ordinal_Order_In_Every_Culture(string cultureName)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUiCulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            var tool = CreateTool();
            var expected = await GenerateAsync(tool);

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);
            var actual = await GenerateAsync(CreateTool());

            await Assert.That(tool.SubDomainGroups.SequenceEqual(
                ["ApplicationInsights", "Applicationcostprofiler", "Zebra", "Ångstrom"])).IsTrue();
            await Assert.That(actual.Select(file => (file.RelativePath, file.Content))
                .SequenceEqual(expected.Select(file => (file.RelativePath, file.Content)))).IsTrue();

            foreach (var file in actual)
            {
                var syntax = CSharpSyntaxTree.ParseText(file.Content).GetRoot();
                await AssertOrdinalOrderAsync(syntax.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                    .Select(property => property.Identifier.ValueText));
                await AssertOrdinalOrderAsync(syntax.DescendantNodes().OfType<MethodDeclarationSyntax>()
                    .Select(method => method.Identifier.ValueText));
                await AssertOrdinalOrderAsync(syntax.DescendantNodes().OfType<FieldDeclarationSyntax>()
                    .SelectMany(field => field.Declaration.Variables)
                    .Select(variable => variable.Identifier.ValueText)
                    .Where(name => name != "_command"));
            }

            var service = actual.Single(file => Path.GetFileName(file.RelativePath) == "Tool.Generated.cs");
            var parameters = CSharpSyntaxTree.ParseText(service.Content).GetRoot().DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>().Single().ParameterList.Parameters;
            await Assert.That(parameters.Select(parameter => parameter.Identifier.ValueText).SequenceEqual(
                ["applicationInsights", "applicationcostprofiler", "zebra", "ångstrom", "command"])).IsTrue();

            var registration = actual.Single(file => Path.GetFileName(file.RelativePath) == "ToolExtensions.Generated.cs");
            var registeredTypes = CSharpSyntaxTree.ParseText(registration.Content).GetRoot().DescendantNodes()
                .OfType<GenericNameSyntax>().Where(name => name.Identifier.ValueText == "TryAddScoped")
                .Skip(1).Select(name => name.TypeArgumentList.Arguments[0].ToString());
            await Assert.That(registeredTypes.SequenceEqual(
                ["IToolApplicationInsights", "IToolApplicationcostprofiler", "IToolZebra", "IToolÅngstrom"])).IsTrue();
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }

    private static async Task AssertOrdinalOrderAsync(IEnumerable<string> names)
    {
        var actual = names.ToArray();
        await Assert.That(string.Join(",", actual)).IsEqualTo(string.Join(",", actual.Order(StringComparer.Ordinal)));
    }

    private static async Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool)
    {
        ICodeGenerator[] generators =
        [
            new ServiceInterfaceGenerator(),
            new ServiceImplementationGenerator(),
            new SubDomainClassGenerator(),
            new GlobalOptionsBaseGenerator(),
            new DependencyRegistrationGenerator(),
        ];
        var files = new List<GeneratedFile>();
        foreach (var generator in generators)
        {
            files.AddRange(await generator.GenerateAsync(tool));
        }

        return files;
    }

    private static CliToolDefinition CreateTool()
    {
        // Mixed case reproduces the AWS ordering defect; Å sorts differently in Swedish.
        string[] names = ["Applicationcostprofiler", "Ångstrom", "Zebra", "ApplicationInsights"];
        var commands = new List<CliCommandDefinition>();
        foreach (var name in names)
        {
            commands.Add(CreateCommand(["root", name], null));
            commands.Add(CreateCommand([name, "show"], name));
            commands.Add(CreateCommand(["zebra", "leaf" + name], "Zebra"));
            commands.Add(CreateCommand(["zebra", name], "Zebra"));
            commands.Add(CreateCommand(["zebra", name, "show"], "Zebra"));
        }

        return new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "output",
            Commands = commands,
            GlobalOptions = [.. names.Select(name => new CliOptionDefinition
            {
                SwitchName = "--" + name,
                PropertyName = name,
                CSharpType = "string?",
            })],
        };
    }

    private static CliCommandDefinition CreateCommand(string[] parts, string? subDomainGroup) => new()
    {
        FullCommand = "tool " + string.Join(" ", parts),
        CommandParts = parts,
        ClassName = "Tool" + string.Concat(parts.Select(GeneratorUtils.ToPascalCase)) + "Options",
        ParentClassName = "ToolOptions",
        ToolNamespacePrefix = "Tool",
        SubDomainGroup = subDomainGroup,
        Options = [],
    };
}
