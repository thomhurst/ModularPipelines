using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class ServiceAwaitGenerationTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Service_Methods_Do_Not_Capture_The_Caller_Context(bool nested, bool required)
    {
        var command = new CliCommandDefinition
        {
            FullCommand = nested ? "tool group run" : "tool run",
            CommandParts = nested ? ["group", "run"] : ["run"],
            ClassName = "ToolRunOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            SubDomainGroup = nested ? "group" : null,
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--id",
                    PropertyName = "Id",
                    CSharpType = "string?",
                    IsRequired = required,
                },
            ],
        };
        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "output",
            Commands = [command],
        };
        ICodeGenerator generator = nested ? new SubDomainClassGenerator() : new ServiceImplementationGenerator();

        var files = await generator.GenerateAsync(tool);
        var service = files.Single(file => file.Content.Contains("return await _command", StringComparison.Ordinal)).Content;
        var options = required ? "options" : "options ?? new ToolRunOptions()";

        await Assert.That(service).Contains(
            $"return await _command.ExecuteCommandLineToolAsync({options}, executionOptions, cancellationToken).ConfigureAwait(false);");
    }
}
