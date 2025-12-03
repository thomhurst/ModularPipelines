using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic", "create")]
public record AzEventgridTopicCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--extended-location-name")]
    public string? ExtendedLocationName { get; set; }

    [CliOption("--extended-location-type")]
    public string? ExtendedLocationType { get; set; }

    [CliOption("--inbound-ip-rules")]
    public string? InboundIpRules { get; set; }

    [CliOption("--input-mapping-default-values")]
    public string? InputMappingDefaultValues { get; set; }

    [CliOption("--input-mapping-fields")]
    public string? InputMappingFields { get; set; }

    [CliOption("--input-schema")]
    public string? InputSchema { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}