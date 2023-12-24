using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-elastic-ips")]
public record AwsOpsworksDescribeElasticIpsOptions : AwsOptions
{
    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--stack-id")]
    public string? StackId { get; set; }

    [CommandSwitch("--ips")]
    public string[]? Ips { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}