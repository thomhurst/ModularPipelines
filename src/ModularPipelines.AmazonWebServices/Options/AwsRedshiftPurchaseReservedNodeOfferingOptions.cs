using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "purchase-reserved-node-offering")]
public record AwsRedshiftPurchaseReservedNodeOfferingOptions(
[property: CommandSwitch("--reserved-node-offering-id")] string ReservedNodeOfferingId
) : AwsOptions
{
    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}