using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic", "create", "(eventgrid", "extension)")]
public record AzEventgridTopicCreateEventgridExtensionOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}