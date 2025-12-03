using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "event-subscription", "list")]
public record AzEventgridEventSubscriptionListOptions : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--source-resource-id")]
    public string? SourceResourceId { get; set; }

    [CliOption("--topic-type-name")]
    public string? TopicTypeName { get; set; }
}