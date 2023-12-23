using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-raid-arrays")]
public record AwsOpsworksDescribeRaidArraysOptions : AwsOptions
{
    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--stack-id")]
    public string? StackId { get; set; }

    [CommandSwitch("--raid-array-ids")]
    public string[]? RaidArrayIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}