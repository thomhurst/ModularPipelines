using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "accept-reserved-node-exchange")]
public record AwsRedshiftAcceptReservedNodeExchangeOptions(
[property: CliOption("--reserved-node-id")] string ReservedNodeId,
[property: CliOption("--target-reserved-node-offering-id")] string TargetReservedNodeOfferingId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}