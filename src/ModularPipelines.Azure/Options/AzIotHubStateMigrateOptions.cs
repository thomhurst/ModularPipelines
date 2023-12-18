using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "state", "migrate")]
public record AzIotHubStateMigrateOptions : AzOptions
{
    [CommandSwitch("--aspects")]
    public string? Aspects { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--destination-hub")]
    public string? DestinationHub { get; set; }

    [CommandSwitch("--destination-hub-login")]
    public string? DestinationHubLogin { get; set; }

    [CommandSwitch("--destination-resource-group")]
    public string? DestinationResourceGroup { get; set; }

    [CommandSwitch("--og")]
    public string? Og { get; set; }

    [CommandSwitch("--oh")]
    public string? Oh { get; set; }

    [CommandSwitch("--ol")]
    public string? Ol { get; set; }

    [BooleanCommandSwitch("--replace")]
    public bool? Replace { get; set; }
}