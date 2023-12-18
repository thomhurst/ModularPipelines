using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "policy", "update")]
public record AzIotDpsPolicyUpdateOptions(
[property: CommandSwitch("--dps-name")] string DpsName,
[property: CommandSwitch("--pn")] string Pn
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--primary-key")]
    public string? PrimaryKey { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rights")]
    public string? Rights { get; set; }

    [CommandSwitch("--secondary-key")]
    public string? SecondaryKey { get; set; }
}

