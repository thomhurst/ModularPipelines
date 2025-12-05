using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "delete-inbound-cross-cluster-search-connection")]
public record AwsEsDeleteInboundCrossClusterSearchConnectionOptions(
[property: CliOption("--cross-cluster-search-connection-id")] string CrossClusterSearchConnectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}