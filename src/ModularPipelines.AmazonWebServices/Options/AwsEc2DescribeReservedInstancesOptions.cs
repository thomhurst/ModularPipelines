using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-reserved-instances")]
public record AwsEc2DescribeReservedInstancesOptions : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--offering-class")]
    public string? OfferingClass { get; set; }

    [CliOption("--reserved-instances-ids")]
    public string[]? ReservedInstancesIds { get; set; }

    [CliOption("--offering-type")]
    public string? OfferingType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}