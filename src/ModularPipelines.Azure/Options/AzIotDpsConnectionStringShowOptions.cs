using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "connection-string", "show")]
public record AzIotDpsConnectionStringShowOptions : AzOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--dps-name")]
    public string? DpsName { get; set; }

    [CommandSwitch("--key-type")]
    public string? KeyType { get; set; }

    [CommandSwitch("--pn")]
    public string? Pn { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}