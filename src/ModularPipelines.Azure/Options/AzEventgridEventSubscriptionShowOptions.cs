using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "event-subscription", "show")]
public record AzEventgridEventSubscriptionShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--include-attrib-secret")]
    public bool? IncludeAttribSecret { get; set; }

    [BooleanCommandSwitch("--include-full-endpoint-url")]
    public bool? IncludeFullEndpointUrl { get; set; }

    [CommandSwitch("--source-resource-id")]
    public string? SourceResourceId { get; set; }
}