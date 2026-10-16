using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Enums;

namespace ModularPipelines.UnitTests.Helpers;

public class SudoCommandTests : TestBase
{
    private sealed class CaptureInvocationInterceptor : ICommandInterceptor
    {
        public CommandInvocation? Invocation { get; private set; }

        public ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken)
        {
            Invocation = invocation;
            return ValueTask.FromResult(CommandResult.Ok());
        }
    }

    [Test]
    public async Task Sudo_Preserves_Only_Explicit_NonNull_Environment_Variables()
    {
        var capture = new CaptureInvocationInterceptor();
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(capture));

        var result = await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("tool with spaces") { Arguments = ["--flag", "argument with spaces", ""] },
            new CommandExecutionOptions
            {
                Sudo = true,
                EnvironmentVariables = new Dictionary<string, string?>(StringComparer.Ordinal)
                {
                    ["PATH"] = "/custom/bin",
                    ["FOO"] = "upper-value",
                    ["bar"] = "lower-value",
                    ["MP_EMPTY"] = "",
                    ["MP_REMOVE"] = null,
                },
            });

        using (Assert.Multiple())
        {
            await Assert.That(capture.Invocation!.CommandLine.Tool).IsEqualTo("sudo");
            await Assert.That(capture.Invocation.CommandLine.Arguments).IsEquivalentTo(
                ["-n", "--preserve-env=PATH,FOO,bar,MP_EMPTY", "--", "tool with spaces", "--flag", "argument with spaces", ""],
                CollectionOrdering.Matching);
            await Assert.That(capture.Invocation.EnvironmentVariables["PATH"]).IsEqualTo("/custom/bin");
            await Assert.That(capture.Invocation.EnvironmentVariables["MP_REMOVE"]).IsNull();
            await Assert.That(result.CommandInput).DoesNotContain("upper-value");
            await Assert.That(result.CommandInput).DoesNotContain("lower-value");
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task Sudo_Without_Variables_To_Preserve_Still_Disables_Prompts(int environmentCase)
    {
        var capture = new CaptureInvocationInterceptor();
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(capture));

        await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("--tool") { Arguments = ["--flag"] },
            new CommandExecutionOptions
            {
                Sudo = true,
                EnvironmentVariables = environmentCase switch
                {
                    0 => null,
                    1 => new Dictionary<string, string?>(),
                    _ => new Dictionary<string, string?> { ["MP_REMOVE"] = null },
                },
            });

        await Assert.That(capture.Invocation!.CommandLine.Tool).IsEqualTo("sudo");
        await Assert.That(capture.Invocation.CommandLine.Arguments).IsEquivalentTo(
            ["-n", "--", "--tool", "--flag"], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Sudo_Rejects_Commas_In_Preserved_Variable_Names()
    {
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(new CaptureInvocationInterceptor()));

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("unused"),
            new CommandExecutionOptions
            {
                Sudo = true,
                EnvironmentVariables = new Dictionary<string, string?> { ["MP_FIRST,MP_SECOND"] = "value" },
            }));

        await Assert.That(exception!.ParamName).IsEqualTo("executionOptions");
    }

    [Test]
    public async Task Without_Sudo_Command_And_Arguments_Are_Unchanged()
    {
        var capture = new CaptureInvocationInterceptor();
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(capture));

        await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("unused") { Arguments = ["--flag", "value"] },
            new CommandExecutionOptions
            {
                EnvironmentVariables = new Dictionary<string, string?> { ["MP_TEST"] = "value" },
            });

        await Assert.That(capture.Invocation!.CommandLine.Tool).IsEqualTo("unused");
        await Assert.That(capture.Invocation.CommandLine.Arguments).IsEquivalentTo(
            ["--flag", "value"], CollectionOrdering.Matching);
    }
}
