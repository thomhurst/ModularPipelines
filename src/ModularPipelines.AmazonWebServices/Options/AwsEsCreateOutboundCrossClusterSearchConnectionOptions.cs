using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "create-outbound-cross-cluster-search-connection")]
public record AwsEsCreateOutboundCrossClusterSearchConnectionOptions(
[property: CliOption("--source-domain-info")] string SourceDomainInfo,
[property: CliOption("--destination-domain-info")] string DestinationDomainInfo,
[property: CliOption("--connection-alias")] string ConnectionAlias
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}