using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain", "event-subscription", "show")]
public record AzEventgridDomainEventSubscriptionShowOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--full-ed-url")]
    public bool? FullEdUrl { get; set; }

    [CliFlag("--include-attrib-secret")]
    public bool? IncludeAttribSecret { get; set; }
}