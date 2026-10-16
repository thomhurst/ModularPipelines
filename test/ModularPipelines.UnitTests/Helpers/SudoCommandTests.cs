using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Enums;

namespace ModularPipelines.UnitTests.Helpers;

public class SudoCommandTests : TestBase
{
    private sealed class ChangeInvocationInterceptor(Func<CommandInvocation, CommandInvocation> change) : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken) => next(change(invocation), cancellationToken);
    }

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
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task Sudo_Refreshes_Preserved_Variables_After_Each_Interceptor(int environmentCase)
    {
        var capture = new CaptureInvocationInterceptor();
        var (command, _) = await GetService<ICommandContext>(services =>
        {
            services.AddSingleton<ICommandInterceptor>(new ChangeInvocationInterceptor(invocation => invocation with
            {
                ExecutionOptions = invocation.ExecutionOptions with
                {
                    EnvironmentVariables = new Dictionary<string, string?> { ["MP_INTERMEDIATE"] = "intermediate-value" },
                },
            }));
            services.AddSingleton<ICommandInterceptor>(new ChangeInvocationInterceptor(invocation => invocation with
            {
                CommandLine = new CommandLine(invocation.CommandLine.Tool, [.. invocation.CommandLine.Arguments, "added argument"]),
                ExecutionOptions = invocation.ExecutionOptions with
                {
                    EnvironmentVariables = environmentCase switch
                    {
                        0 => new Dictionary<string, string?> { ["MP_ADDED"] = "added-value", ["MP_EMPTY"] = "", ["MP_REMOVE"] = null },
                        1 => new Dictionary<string, string?> { ["MP_REMOVE"] = null },
                        _ => null,
                    },
                },
            }));
            services.AddSingleton<ICommandInterceptor>(capture);
        });

        var result = await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("tool with spaces") { Arguments = ["--flag", ""] },
            new CommandExecutionOptions
            {
                Sudo = true,
                EnvironmentVariables = new Dictionary<string, string?> { ["MP_ORIGINAL"] = "original-value" },
            });

        var expectedArguments = new List<string> { "-n" };
        if (environmentCase == 0)
        {
            expectedArguments.Add("--preserve-env=MP_ADDED,MP_EMPTY");
        }

        expectedArguments.AddRange(["--", "tool with spaces", "--flag", "", "added argument"]);
        await Assert.That(capture.Invocation!.CommandLine.Tool).IsEqualTo("sudo");
        await Assert.That(capture.Invocation.CommandLine.Arguments).IsEquivalentTo(expectedArguments, CollectionOrdering.Matching);
        await Assert.That(result.CommandInput).DoesNotContain("MP_ORIGINAL");
        await Assert.That(result.CommandInput).DoesNotContain("MP_INTERMEDIATE");
        await Assert.That(result.CommandInput).DoesNotContain("added-value");
        if (environmentCase == 0)
        {
            await Assert.That(result.EnvironmentVariables["MP_ADDED"]).IsEqualTo("added-value");
            await Assert.That(result.EnvironmentVariables["MP_EMPTY"]).IsEqualTo("");
            await Assert.That(result.EnvironmentVariables["MP_REMOVE"]).IsNull();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Sudo_Uses_Interceptor_Environment_In_Final_Command(bool hasOriginalEnvironment)
    {
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(new ChangeInvocationInterceptor(invocation => invocation with
            {
                ExecutionOptions = invocation.ExecutionOptions with
                {
                    EnvironmentVariables = new Dictionary<string, string?> { ["MP_ADDED"] = "added-value" },
                },
            })));

        var result = await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("unused") { Arguments = ["--preserve-env=tool-argument"] },
            new CommandExecutionOptions
            {
                Sudo = true,
                InternalDryRun = true,
                EnvironmentVariables = hasOriginalEnvironment
                    ? new Dictionary<string, string?> { ["MP_ORIGINAL"] = "original-value" }
                    : null,
            });

        await Assert.That(result.CommandInput).Contains("--preserve-env=MP_ADDED");
        await Assert.That(result.CommandInput).Contains("--preserve-env=tool-argument");
        await Assert.That(result.CommandInput).DoesNotContain("MP_ORIGINAL");
        await Assert.That(result.CommandInput).DoesNotContain("added-value");
        await Assert.That(result.EnvironmentVariables["MP_ADDED"]).IsEqualTo("added-value");
    }

    [Test]
    public async Task Sudo_Rejects_Commas_Added_By_Interceptor_Before_Execution()
    {
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor>(new ChangeInvocationInterceptor(invocation => invocation with
            {
                ExecutionOptions = invocation.ExecutionOptions with
                {
                    EnvironmentVariables = new Dictionary<string, string?> { ["MP_FIRST,MP_SECOND"] = "value" },
                },
            })));

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("unused"),
            new CommandExecutionOptions { Sudo = true, InternalDryRun = true }));

        await Assert.That(exception!.ParamName).IsEqualTo("executionOptions");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Sudo_Leaves_Interceptor_Command_Line_Replacements_Unchanged(bool replaceSudoArguments)
    {
        var capture = new CaptureInvocationInterceptor();
        var replacement = replaceSudoArguments
            ? new CommandLine("sudo", ["-n", "--", "replacement", "--preserve-env=tool-argument"])
            : new CommandLine("replacement", ["argument"]);
        var (command, _) = await GetService<ICommandContext>(services =>
        {
            services.AddSingleton<ICommandInterceptor>(new ChangeInvocationInterceptor(invocation => invocation with
            {
                CommandLine = replacement,
                ExecutionOptions = invocation.ExecutionOptions with
                {
                    EnvironmentVariables = new Dictionary<string, string?> { ["MP_ADDED"] = "value" },
                },
            }));
            services.AddSingleton<ICommandInterceptor>(capture);
        });

        await command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions("original"),
            new CommandExecutionOptions
            {
                Sudo = true,
                EnvironmentVariables = new Dictionary<string, string?> { ["MP_ORIGINAL"] = "value" },
            });

        await Assert.That(capture.Invocation!.CommandLine).IsEqualTo(replacement);
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
