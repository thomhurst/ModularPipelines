using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class ConstructorDocumentationTests
{
    [Test]
    [Arguments(false, "Create an item.")]
    [Arguments(true, "Create an item.")]
    [Arguments(false, null)]
    [Arguments(true, null)]
    [Arguments(false, "")]
    [Arguments(true, "")]
    [Arguments(false, "  ")]
    [Arguments(true, "  ")]
    [Arguments(false, "Create an item.", true)]
    [Arguments(true, null, true)]
    public async Task Required_Parameters_Preserve_Escaped_Help_Descriptions(bool positional, string? commandDescription, bool primaryConstructor = false)
    {
        const string description = "Overrides 'TF_STACKS_ORGANIZATION_NAME'.\nUse <name> & account (required).";
        var command = new CliCommandDefinition
        {
            FullCommand = "tool create",
            CommandParts = ["create"],
            ClassName = "ToolCreateOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Description = commandDescription,
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--name",
                    PropertyName = positional ? "OptionalName" : "Name",
                    CSharpType = primaryConstructor ? "int?" : "string?",
                    Description = description,
                    IsRequired = !positional,
                },
                new CliOptionDefinition
                {
                    SwitchName = "--id",
                    PropertyName = "Id",
                    CSharpType = primaryConstructor ? "int" : "string",
                    IsRequired = true,
                },
            ],
            PositionalArguments = positional
                ? [new CliPositionalArgument { PropertyName = "Name", CSharpType = primaryConstructor ? "int" : "string", Description = description, IsRequired = true }]
                : [],
        };
        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "output",
            Commands = [command],
        };

        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        var declaration = CSharpSyntaxTree.ParseText(generated).GetRoot()
            .DescendantNodes().OfType<RecordDeclarationSyntax>().Single();
        var documentation = ReadDocumentation(declaration);
        await Assert.That(documentation.Root!.Elements().First().Name.LocalName).IsEqualTo("summary");
        await Assert.That(documentation.Root.Element("summary")!.Value.Trim())
            .IsEqualTo(string.IsNullOrWhiteSpace(commandDescription) ? "Options for tool create." : commandDescription);
        SyntaxNode parameterOwner = primaryConstructor
            ? declaration
            : declaration.Members.OfType<ConstructorDeclarationSyntax>().Single();
        var parameters = ReadDocumentation(parameterOwner).Descendants("param")
            .ToDictionary(parameter => parameter.Attribute("name")!.Value);
        await Assert.That(documentation.Descendants("param")).Count().IsEqualTo(primaryConstructor ? 2 : 0);

        await Assert.That(parameters.Keys).IsEquivalentTo(["Name", "Id"]);
        await Assert.That(parameters["Name"].Value).IsEqualTo(description.Replace('\n', ' '));
        await Assert.That(parameters["Id"].Value).IsEmpty();
        await Assert.That(primaryConstructor ? declaration.ParameterList is not null : declaration.ParameterList is null).IsTrue();
    }

    private static XDocument ReadDocumentation(SyntaxNode node) =>
        XDocument.Parse("<doc>" + string.Join("\n", node.GetLeadingTrivia().ToFullString().Split('\n')
            .Select(line => line.TrimStart())
            .Where(line => line.StartsWith("///", StringComparison.Ordinal))
            .Select(line => line[3..])) + "</doc>");
}
