using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "describe-ec2-instance-limits")]
public record AwsGameliftDescribeEc2InstanceLimitsOptions : AwsOptions
{
    [CliOption("--ec2-instance-type")]
    public string? Ec2InstanceType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}