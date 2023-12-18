using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "task", "run")]
public record AzAcrTaskRunOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--agent-pool")]
    public string? AgentPool { get; set; }

    [CommandSwitch("--arg")]
    public string? Arg { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

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

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secret-arg")]
    public string? SecretArg { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--set-secret")]
    public string? SetSecret { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--update-trigger-token")]
    public string? UpdateTriggerToken { get; set; }
}

