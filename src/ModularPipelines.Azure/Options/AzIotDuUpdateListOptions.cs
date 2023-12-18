using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "update", "list")]
public record AzIotDuUpdateListOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--instance")] string Instance
) : AzOptions
{
    [BooleanCommandSwitch("--by-provider")]
    public bool? ByProvider { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--search")]
    public string? Search { get; set; }

    [CommandSwitch("--un")]
    public string? Un { get; set; }

    [CommandSwitch("--up")]
    public string? Up { get; set; }
}