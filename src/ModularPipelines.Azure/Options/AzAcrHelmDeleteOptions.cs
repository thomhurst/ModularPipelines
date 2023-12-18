using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "helm", "delete")]
public record AzAcrHelmDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--prov")]
    public bool? Prov { get; set; }

    [CommandSwitch("--suffix")]
    public string? Suffix { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? CHART { get; set; }
}

