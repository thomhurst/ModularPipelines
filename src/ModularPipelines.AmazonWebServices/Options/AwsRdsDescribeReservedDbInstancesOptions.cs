using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "describe-reserved-db-instances")]
public record AwsRdsDescribeReservedDbInstancesOptions : AwsOptions
{
    [CommandSwitch("--reserved-db-instance-id")]
    public string? ReservedDbInstanceId { get; set; }

    [CommandSwitch("--reserved-db-instances-offering-id")]
    public string? ReservedDbInstancesOfferingId { get; set; }

    [CommandSwitch("--db-instance-class")]
    public string? DbInstanceClass { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [CommandSwitch("--product-description")]
    public string? ProductDescription { get; set; }

    [CommandSwitch("--offering-type")]
    public string? OfferingType { get; set; }

    [CommandSwitch("--lease-id")]
    public string? LeaseId { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}