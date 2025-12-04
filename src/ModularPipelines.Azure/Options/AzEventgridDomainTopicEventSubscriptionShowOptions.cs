using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "domain", "topic", "event-subscription", "show")]
public record AzEventgridDomainTopicEventSubscriptionShowOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--domain-topic-name")] string DomainTopicName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--full-ed-url")]
    public bool? FullEdUrl { get; set; }

    [CliFlag("--include-attrib-secret")]
    public bool? IncludeAttribSecret { get; set; }
}