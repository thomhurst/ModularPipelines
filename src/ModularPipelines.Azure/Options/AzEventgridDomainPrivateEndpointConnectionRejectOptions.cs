using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "domain", "private-endpoint-connection", "reject")]
public record AzEventgridDomainPrivateEndpointConnectionRejectOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}