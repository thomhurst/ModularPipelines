using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "service", "create")]
public record AzPeeringServiceCreateOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--peering-service-name")] string PeeringServiceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--peering-service-location")]
    public string? PeeringServiceLocation { get; set; }

    [CommandSwitch("--peering-service-provider")]
    public string? PeeringServiceProvider { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}