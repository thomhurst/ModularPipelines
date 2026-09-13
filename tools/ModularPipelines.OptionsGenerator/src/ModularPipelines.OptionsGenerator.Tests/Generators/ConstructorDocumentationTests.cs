using System.Xml.Linq;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class ConstructorDocumentationTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Required_Parameters_Preserve_Escaped_Help_Descriptions(bool positional)
    {
        const string description = "Overrides 'TF_STACKS_ORGANIZATION_NAME'.\nUse <name> & account (required).";
        var command = new CliCommandDefinition
        {
            FullCommand = "tool create",
            CommandParts = ["create"],
            ClassName = "ToolCreateOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Description = "Create an item.",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--name",
                    PropertyName = positional ? "OptionalName" : "Name",
                    CSharpType = "string?",
                    Description = description,
                    IsRequired = !positional,
                },
                new CliOptionDefinition
                {
                    SwitchName = "--id",
                    PropertyName = "Id",
                    CSharpType = "string",
                    IsRequired = true,
                },
            ],
            PositionalArguments = positional
                ? [new CliPositionalArgument { PropertyName = "Name", CSharpType = "string", Description = description, IsRequired = true }]
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
        var documentation = XDocument.Parse("<doc>" + string.Join("\n", generated.Split('\n')
            .Where(line => line.StartsWith("///", StringComparison.Ordinal))
            .Select(line => line[3..])) + "</doc>");
        var parameters = documentation.Descendants("param").ToDictionary(parameter => parameter.Attribute("name")!.Value);

        await Assert.That(parameters.Keys).IsEquivalentTo(["Name", "Id"]);
        await Assert.That(parameters["Name"].Value).IsEqualTo(description.Replace('\n', ' '));
        await Assert.That(parameters["Id"].Value).IsEmpty();
        await Assert.That(generated.IndexOf("<param", StringComparison.Ordinal))
            .IsLessThan(generated.IndexOf("[GeneratedCode", StringComparison.Ordinal));
    }
}
