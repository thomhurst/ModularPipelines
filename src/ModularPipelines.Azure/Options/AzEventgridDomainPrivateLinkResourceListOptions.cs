using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain", "private-link-resource", "list")]
public record AzEventgridDomainPrivateLinkResourceListOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;