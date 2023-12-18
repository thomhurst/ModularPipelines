using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic", "create")]
public record AzEventgridTopicCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--extended-location-name")]
    public string? ExtendedLocationName { get; set; }

    [CommandSwitch("--extended-location-type")]
    public string? ExtendedLocationType { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--inbound-ip-rules")]
    public string? InboundIpRules { get; set; }

    [CommandSwitch("--input-mapping-default-values")]
    public string? InputMappingDefaultValues { get; set; }

    [CommandSwitch("--input-mapping-fields")]
    public string? InputMappingFields { get; set; }

    [CommandSwitch("--input-schema")]
    public string? InputSchema { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

