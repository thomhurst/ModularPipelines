using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests;

public class CommandTimeoutTests
{
    [Test]
    public async Task Type_Enhancement_Uses_The_Configured_Command_Executor()
    {
        var executor = new RecordingExecutor();
        using var services = new ServiceCollection()
            .AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance)
            .AddSingleton<ICliCommandExecutor>(executor)
            .AddSingleton(OptionsGeneratorCommand.CreateTypeEnhancer)
            .BuildServiceProvider();
        var tool = new CliToolDefinition
        {
            ToolName = "helm",
            NamespacePrefix = "Helm",
            TargetNamespace = "ModularPipelines.Helm",
            OutputDirectory = "src/ModularPipelines.Helm",
            Commands = [new()
            {
                FullCommand = "helm codex-type-probe",
                CommandParts = ["codex-type-probe"],
                ClassName = "HelmProbeOptions",
                ParentClassName = "HelmOptions",
                ToolNamespacePrefix = "Helm",
                Options = [new() { SwitchName = "--probe", PropertyName = "Probe", CSharpType = "string?" }],
            }],
        };
        var enhanced = await services.GetRequiredService<OptionTypeEnhancer>().EnhanceAsync(tool);

        await Assert.That(executor.Commands).IsEquivalentTo(["helm codex-type-probe --help"]);
        await Assert.That(enhanced.Commands.Single().Options.Single().CSharpType).IsEqualTo("int?");
    }

    [Test]
    [Arguments("1")]
    [Arguments("180")]
    [Arguments("600")]
    public async Task Accepts_Bounded_Command_Timeout(string seconds)
    {
        var result = await OptionsGeneratorCommand.RunAsync(["--list-tools", "--json", "--command-timeout-seconds", seconds]);

        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    [Arguments("0")]
    [Arguments("-1")]
    [Arguments("601")]
    public async Task Rejects_Out_Of_Range_Command_Timeout(string seconds)
    {
        var result = await OptionsGeneratorCommand.RunAsync(["--list-tools", "--json", "--command-timeout-seconds", seconds]);

        await Assert.That(result).IsNotEqualTo(0);
    }

    private sealed class RecordingExecutor : ICliCommandExecutor
    {
        public List<string> Commands { get; } = [];

        public Task<CliCommandResult> ExecuteAsync(
            string command, string arguments, CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            Commands.Add($"{command} {arguments}");
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = "Flags:\n  --probe int   Probe value",
                StandardError = string.Empty,
                ExitCode = 0,
            });
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
