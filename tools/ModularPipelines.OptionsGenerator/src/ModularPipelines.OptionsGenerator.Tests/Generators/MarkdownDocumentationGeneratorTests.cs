using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class MarkdownDocumentationGeneratorTests
{
    [Test]
    public async Task GenerateAsync_EmitsInstallEntryPointCommandsAndWorkedExample()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake-cli",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands =
            [
                new CliCommandDefinition
                {
                    FullCommand = "fake-cli run",
                    CommandParts = ["run"],
                    ClassName = "FakeRunOptions",
                    ParentClassName = "FakeOptions",
                    ToolNamespacePrefix = "Fake",
                    Options =
                    [
                        new CliOptionDefinition
                        {
                            SwitchName = "--project",
                            PropertyName = "Project",
                            CSharpType = "string",
                            IsRequired = true,
                        },
                    ],
                },
            ],
        };

        var files = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(files).Count().IsEqualTo(1);
        await Assert.That(files[0].RelativePath.Replace('\\', '/'))
            .IsEqualTo("docs/docs/mp-packages/cli/fake-cli.md");
        await Assert.That(files[0].Content).Contains("dotnet add package ModularPipelines.Fake");
        await Assert.That(files[0].Content).Contains("context.Fake()");
        await Assert.That(files[0].Content).Contains("| `fake-cli run` | `FakeRunOptions` |");
        await Assert.That(files[0].Content).Contains("new FakeRunOptions(\"value\")");
    }

    [Test]
    public async Task GenerateAsync_ExcludesRootCommandsHiddenBySubDomainProperties()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands =
            [
                Command("fake app", "FakeAppOptions", ["app"]),
                Command("fake app get", "FakeAppGetOptions", ["app", "get"], "App"),
            ],
        };

        var files = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(files[0].Content).DoesNotContain("| `fake app` | `FakeAppOptions` |");
        await Assert.That(files[0].Content).Contains("| `fake app get` | `FakeAppGetOptions` |");
    }

    private static CliCommandDefinition Command(
        string fullCommand,
        string className,
        string[] commandParts,
        string? subDomainGroup = null) => new()
        {
            FullCommand = fullCommand,
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = "FakeOptions",
            ToolNamespacePrefix = "Fake",
            Options = [],
            SubDomainGroup = subDomainGroup,
        };
}
