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
        await Assert.That(files[0].Content).Contains("using ModularPipelines.Context;");
        await Assert.That(files[0].Content).Contains("using ModularPipelines.Models;");
        await Assert.That(files[0].Content).Contains("using ModularPipelines.Modules;");
        await Assert.That(files[0].Content).Contains("Module<CommandResult>");
        await Assert.That(files[0].Content).Contains("Task<CommandResult?> ExecuteAsync");
        await Assert.That(files[0].Content).Contains("return await context.Fake()");
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

    [Test]
    public async Task GenerateAsync_UsesTheGeneratedConstructorParameterList()
    {
        var command = Command("fake run", "FakeRunOptions", ["run"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--target",
                    PropertyName = "Target",
                    CSharpType = "int",
                    IsRequired = true,
                },
            ],
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "target",
                    CSharpType = "string",
                    IsRequired = true,
                },
                new CliPositionalArgument
                {
                    PropertyName = "Files",
                    CSharpType = "string",
                    IsRequired = true,
                },
                new CliPositionalArgument
                {
                    PropertyName = "files",
                    CSharpType = "IEnumerable<string>",
                    IsRequired = true,
                },
            ],
        };
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = [command],
        };

        var documentation = await new MarkdownDocumentationGenerator().GenerateAsync(tool);
        var options = await new OptionsClassGenerator().GenerateAsync(tool);

        await Assert.That(documentation[0].Content).Contains("new FakeRunOptions(1, [\"value\"])");
        await Assert.That(options[0].Content).Contains("int Target");
        await Assert.That(options[0].Content).Contains("IEnumerable<string> Files");
        await Assert.That(options[0].Content).DoesNotContain("string target");
    }

    [Test]
    public async Task GenerateAsync_SelectsExampleCommandDeterministically()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands =
            [
                Command("fake zeta", "FakeZetaOptions", ["zeta"]),
                Command("fake alpha", "FakeAlphaOptions", ["alpha"]),
            ],
        };

        var documentation = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(documentation[0].Content).Contains("context.Fake().Alpha(");
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
