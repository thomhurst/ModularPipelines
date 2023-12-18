using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "connection-string", "show")]
public record AzIotHubConnectionStringShowOptions : AzOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--default-eventhub")]
    public bool? DefaultEventhub { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--key-type")]
    public string? KeyType { get; set; }

    [CommandSwitch("--pn")]
    public string? Pn { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}