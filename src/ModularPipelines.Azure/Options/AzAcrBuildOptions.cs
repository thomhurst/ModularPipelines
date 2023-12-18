using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "build")]
public record AzAcrBuildOptions(
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--agent-pool")]
    public string? AgentPool { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--build-arg")]
    public IEnumerable<KeyValue>? BuildArg { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--log-template")]
    public string? LogTemplate { get; set; }

    [BooleanCommandSwitch("--no-format")]
    public bool? NoFormat { get; set; }

    [BooleanCommandSwitch("--no-logs")]
    public bool? NoLogs { get; set; }

    [BooleanCommandSwitch("--no-push")]
    public bool? NoPush { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secret-build-arg")]
    public string? SecretBuildArg { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SourceLocation { get; set; }
}