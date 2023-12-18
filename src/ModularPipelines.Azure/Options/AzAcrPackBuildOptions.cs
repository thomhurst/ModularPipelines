using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "pack", "build")]
public record AzAcrPackBuildOptions(
[property: CommandSwitch("--builder")] string Builder,
[property: CommandSwitch("--image")] string Image,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--agent-pool")]
    public string? AgentPool { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [BooleanCommandSwitch("--no-format")]
    public bool? NoFormat { get; set; }

    [BooleanCommandSwitch("--no-logs")]
    public bool? NoLogs { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pack-image-tag")]
    public string? PackImageTag { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [BooleanCommandSwitch("--pull")]
    public bool? Pull { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SourceLocation { get; set; }
}