using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "topic", "create", "(eventgrid", "extension)")]
public record AzEventgridTopicCreateEventgridExtensionOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--inbound-ip-rules")]
    public string? InboundIpRules { get; set; }

    [CliOption("--input-mapping-default-values")]
    public string? InputMappingDefaultValues { get; set; }

    [CliOption("--input-mapping-fields")]
    public string? InputMappingFields { get; set; }

    [CliOption("--input-schema")]
    public string? InputSchema { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}