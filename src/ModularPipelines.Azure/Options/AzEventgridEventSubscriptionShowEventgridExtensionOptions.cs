using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "event-subscription", "show", "(eventgrid", "extension)")]
public record AzEventgridEventSubscriptionShowEventgridExtensionOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--include-full-endpoint-url")]
    public bool? IncludeFullEndpointUrl { get; set; }

    [CliOption("--source-resource-id")]
    public string? SourceResourceId { get; set; }
}