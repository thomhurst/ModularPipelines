using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class PositionalDocumentationTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Examples_Follow_Renamed_Operands_Without_Reassigning_Local_Option_Values(bool localOption)
    {
        var expression = new CliOptionDefinition
        {
            SwitchName = "--expression",
            PropertyName = "Expression",
            CSharpType = "string?",
        };
        var command = new CliCommandDefinition
        {
            FullCommand = "fake eval",
            CommandParts = ["eval"],
            ClassName = "FakeEvalOptions",
            ParentClassName = "FakeOptions",
            ToolNamespacePrefix = "Fake",
            Options = localOption ? [expression] : [],
            PositionalArguments = [new CliPositionalArgument
            {
                PropertyName = "Expression",
                CSharpType = "string?",
                PositionIndex = 0,
            }],
            IsSafeForDocumentation = true,
            DocumentationExampleValues = new Dictionary<string, string>
            {
                ["Expression"] = "\".items\"",
            },
        };
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = [command],
            GlobalOptions = [expression],
            PreferredDocumentationExampleCommand = "fake eval",
        };
        var resolved = InheritedPropertyCollisionResolver.Resolve(tool);
        var expectedKey = localOption ? "Expression" : "ExpressionArgument";
        await Assert.That(resolved.Commands.Single().DocumentationExampleValues.Keys).IsEquivalentTo([expectedKey]);
        var markdown = (await new MarkdownDocumentationGenerator().GenerateAsync(resolved)).Single().Content;
        await Assert.That(markdown).Contains(expectedKey + " = \".items\"");
        var again = InheritedPropertyCollisionResolver.Resolve(resolved);
        await Assert.That(again.Commands.Single().DocumentationExampleValues.Keys).IsEquivalentTo([expectedKey]);
    }
}
