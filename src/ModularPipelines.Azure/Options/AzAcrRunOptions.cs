using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "run")]
public record AzAcrRunOptions(
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--agent-pool")]
    public string? AgentPool { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--cmd")]
    public string? Cmd { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--log-template")]
    public string? LogTemplate { get; set; }

    [BooleanCommandSwitch("--no-format")]
    public bool? NoFormat { get; set; }

    [BooleanCommandSwitch("--no-logs")]
    public bool? NoLogs { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--set-secret")]
    public string? SetSecret { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--values")]
    public string? Values { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SourceLocation { get; set; }
}

