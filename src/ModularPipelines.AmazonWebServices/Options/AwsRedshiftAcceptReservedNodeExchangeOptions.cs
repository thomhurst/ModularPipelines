using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "accept-reserved-node-exchange")]
public record AwsRedshiftAcceptReservedNodeExchangeOptions(
[property: CommandSwitch("--reserved-node-id")] string ReservedNodeId,
[property: CommandSwitch("--target-reserved-node-offering-id")] string TargetReservedNodeOfferingId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}