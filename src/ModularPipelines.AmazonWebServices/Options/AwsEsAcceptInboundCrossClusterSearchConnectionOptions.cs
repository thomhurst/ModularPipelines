using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "accept-inbound-cross-cluster-search-connection")]
public record AwsEsAcceptInboundCrossClusterSearchConnectionOptions(
[property: CliOption("--cross-cluster-search-connection-id")] string CrossClusterSearchConnectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}