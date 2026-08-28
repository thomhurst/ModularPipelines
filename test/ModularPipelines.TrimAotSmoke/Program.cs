using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Configuration;
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

// Exercise OptionsProvider's runtime-discovered IOptions<T> path after trimming.
builder.Services.AddSingleton<IOptions<SmokePipelineOptions>>(
    Options.Create(new SmokePipelineOptions { Marker = SmokeState.OptionsMarker }));
builder
    .AddModule<CommandModule>()
    .AddModule<VerificationModule>()
    .AddModule<IgnoredValueModule>();
builder.IgnoreCategories("ignored");

await using var pipeline = await builder.BuildAsync();
await pipeline.RunAsync();

if (SmokeState.HookInvocations != 3)
{
    throw new InvalidOperationException(
        $"Expected three generated hook invocations, got {SmokeState.HookInvocations}.");
}

using var failureBuilder = Pipeline.CreateBuilder(args);
failureBuilder
    .AddModule<FailingModule>()
    .AddModule<PendingAfterFailureModule>();
failureBuilder.ConfigurePipelineOptions(options => options with
{
    ThrowOnPipelineFailure = true,
});

await using var failurePipeline = await failureBuilder.BuildAsync();
try
{
    await failurePipeline.RunAsync();
    throw new InvalidOperationException("Expected the failure smoke pipeline to throw.");
}
catch (Exception exception) when (
    exception.ToString().Contains(SmokeState.ExpectedFailure, StringComparison.Ordinal))
{
}

Console.WriteLine("TRIM_AOT_SMOKE_OK");

internal static class SmokeState
{
    private static int _hookInvocations;

    public const string ChildArgument = "--aot-smoke-child";

    public const string ExpectedFailure = "trim-aot-expected-failure";

    public const string Secret = "trim-aot-smoke-secret";

    public const string OptionsMarker = "trim-aot-options-marker";

    public static int HookInvocations => Volatile.Read(ref _hookInvocations);

    public static void RecordHookInvocation()
    {
        Interlocked.Increment(ref _hookInvocations);
    }
}

internal sealed class SmokePipelineOptions
{
    public string? Marker { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
internal sealed class SmokeHookAttribute : Attribute, IModuleStartHandler
{
    public Task OnModuleStartAsync(IModuleHookContext context)
    {
        SmokeState.RecordHookInvocation();
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
    protected override Task<CommandResult> ExecuteAsync(
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

        return context.Shell.RunAsync(
            options,
            cancellationToken: cancellationToken)!;
    }
}

[DependsOn<CommandModule>]
[DependsOn<ClosedGenericModule<int>>]
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

internal sealed class IgnoredValueModule : Module<int>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithCategory("ignored")
        .Build();

    protected override Task<int> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        throw new InvalidOperationException("Ignored module should not execute.");
    }
}

[SmokeHook]
[DependsOn<TransitiveGenericModule<string>>]
internal sealed class ClosedGenericModule<T> : Module<bool>
{
    protected override Task<bool> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}

[SmokeHook]
internal sealed class TransitiveGenericModule<T> : Module<bool>
{
    protected override Task<bool> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}

internal sealed class FailingModule : Module<bool>
{
    protected override Task<bool> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        throw new InvalidOperationException(SmokeState.ExpectedFailure);
    }
}

[DependsOn<FailingModule>]
internal sealed class PendingAfterFailureModule : Module<int>
{
    protected override Task<int> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        throw new InvalidOperationException("Pending module should not execute.");
    }
}
