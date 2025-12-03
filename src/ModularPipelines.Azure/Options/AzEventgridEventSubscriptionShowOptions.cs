using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "event-subscription", "show")]
public record AzEventgridEventSubscriptionShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--include-attrib-secret")]
    public bool? IncludeAttribSecret { get; set; }

    [CliFlag("--include-full-endpoint-url")]
    public bool? IncludeFullEndpointUrl { get; set; }

    [CliOption("--source-resource-id")]
    public string? SourceResourceId { get; set; }
}