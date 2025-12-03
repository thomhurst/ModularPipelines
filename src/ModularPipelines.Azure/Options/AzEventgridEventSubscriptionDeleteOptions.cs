using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "event-subscription", "delete")]
public record AzEventgridEventSubscriptionDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--source-resource-id")]
    public string? SourceResourceId { get; set; }
}