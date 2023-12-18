using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "event-subscription", "show", "(eventgrid", "extension)")]
public record AzEventgridEventSubscriptionShowEventgridExtensionOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--include-full-endpoint-url")]
    public bool? IncludeFullEndpointUrl { get; set; }

    [CommandSwitch("--source-resource-id")]
    public string? SourceResourceId { get; set; }
}