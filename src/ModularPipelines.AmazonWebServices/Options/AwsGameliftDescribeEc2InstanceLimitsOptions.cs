using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "describe-ec2-instance-limits")]
public record AwsGameliftDescribeEc2InstanceLimitsOptions : AwsOptions
{
    [CommandSwitch("--ec2-instance-type")]
    public string? Ec2InstanceType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}