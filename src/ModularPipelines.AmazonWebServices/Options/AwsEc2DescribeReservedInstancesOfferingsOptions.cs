using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-reserved-instances-offerings")]
public record AwsEc2DescribeReservedInstancesOfferingsOptions : AwsOptions
{
    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--max-duration")]
    public long? MaxDuration { get; set; }

    [CommandSwitch("--max-instance-count")]
    public int? MaxInstanceCount { get; set; }

    [CommandSwitch("--min-duration")]
    public long? MinDuration { get; set; }

    [CommandSwitch("--offering-class")]
    public string? OfferingClass { get; set; }

    [CommandSwitch("--product-description")]
    public string? ProductDescription { get; set; }

    [CommandSwitch("--reserved-instances-offering-ids")]
    public string[]? ReservedInstancesOfferingIds { get; set; }

    [CommandSwitch("--instance-tenancy")]
    public string? InstanceTenancy { get; set; }

    [CommandSwitch("--offering-type")]
    public string? OfferingType { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}