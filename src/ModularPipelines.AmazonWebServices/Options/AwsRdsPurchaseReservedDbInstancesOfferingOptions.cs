using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "purchase-reserved-db-instances-offering")]
public record AwsRdsPurchaseReservedDbInstancesOfferingOptions(
[property: CommandSwitch("--reserved-db-instances-offering-id")] string ReservedDbInstancesOfferingId
) : AwsOptions
{
    [CommandSwitch("--reserved-db-instance-id")]
    public string? ReservedDbInstanceId { get; set; }

    [CommandSwitch("--db-instance-count")]
    public int? DbInstanceCount { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}