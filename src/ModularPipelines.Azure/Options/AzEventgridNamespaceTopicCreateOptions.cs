using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "namespace", "topic", "create")]
public record AzEventgridNamespaceTopicCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--event-retention-in-days")]
    public string? EventRetentionInDays { get; set; }

    [CliOption("--input-schema")]
    public string? InputSchema { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--publisher-type")]
    public string? PublisherType { get; set; }
}