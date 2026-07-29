using Microsoft.Extensions.DependencyInjection;
using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

if (args is [SmokeState.ChildArgument, var childValue])
{
    Console.WriteLine(childValue);
    return;
}

using var builder = Pipeline.CreateBuilder(args);
builder
    .AddModule<CommandModule>()
    .AddModule<VerificationModule>();

await using var pipeline = await builder.BuildAsync();
await pipeline.RunAsync();

if (SmokeState.HookInvocations != 1)
{
    throw new InvalidOperationException(
        $"Expected one generated hook invocation, got {SmokeState.HookInvocations}.");
}

Console.WriteLine("TRIM_AOT_SMOKE_OK");

internal static class SmokeState
{
    public const string ChildArgument = "--aot-smoke-child";

    public const string Secret = "trim-aot-smoke-secret";

    public static int HookInvocations { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
internal sealed class SmokeHookAttribute : Attribute, IModuleStartHandler
{
    public Task OnModuleStartAsync(IModuleHookContext context)
    {
        SmokeState.HookInvocations++;
        return Task.CompletedTask;
    }
}

internal sealed record SmokeCommandOptions : CommandLineToolOptions
{
    [CliArgument(0)]
    public string? Mode { get; init; }

    [CliArgument(1)]
    [SecretValue]
    public string? Secret { get; init; }
}

[SmokeHook]
internal sealed class CommandModule : Module<CommandResult>
{
    protected override Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var options = new SmokeCommandOptions
        {
            Tool = Environment.ProcessPath
                ?? throw new InvalidOperationException("The current executable path is unavailable."),
            Mode = SmokeState.ChildArgument,
            Secret = SmokeState.Secret,
        };

        return context.Shell.Command.ExecuteCommandLineToolAsync(
            options,
            cancellationToken: cancellationToken)!;
    }
}

[DependsOn<CommandModule>]
internal sealed class VerificationModule(
    ISecretObfuscator secretObfuscator) : Module<bool>
{
    protected override async Task<bool> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var result = await context.GetModule<CommandModule>();
        var command = result.ValueOrDefault
            ?? throw new InvalidOperationException("Command module returned no value.");

        if (command.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Child command exited with {command.ExitCode}: {command.StandardError}");
        }

        var masked = secretObfuscator.Obfuscate(command.StandardOutput, null);
        if (masked.Contains(SmokeState.Secret, StringComparison.Ordinal)
            || !masked.Contains("********", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Secret masking did not redact command output.");
        }

        return true;
    }
}
