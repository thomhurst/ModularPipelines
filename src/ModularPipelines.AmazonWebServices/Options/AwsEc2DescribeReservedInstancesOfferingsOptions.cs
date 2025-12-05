using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-reserved-instances-offerings")]
public record AwsEc2DescribeReservedInstancesOfferingsOptions : AwsOptions
{
    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--max-duration")]
    public long? MaxDuration { get; set; }

    [CliOption("--max-instance-count")]
    public int? MaxInstanceCount { get; set; }

    [CliOption("--min-duration")]
    public long? MinDuration { get; set; }

    [CliOption("--offering-class")]
    public string? OfferingClass { get; set; }

    [CliOption("--product-description")]
    public string? ProductDescription { get; set; }

    [CliOption("--reserved-instances-offering-ids")]
    public string[]? ReservedInstancesOfferingIds { get; set; }

    [CliOption("--instance-tenancy")]
    public string? InstanceTenancy { get; set; }

    [CliOption("--offering-type")]
    public string? OfferingType { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}