using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "describe-stack-resources")]
public record AwsCloudformationDescribeStackResourcesOptions : AwsOptions
{
    [CommandSwitch("--stack-name")]
    public string? StackName { get; set; }

    [CommandSwitch("--logical-resource-id")]
    public string? LogicalResourceId { get; set; }

    [CommandSwitch("--physical-resource-id")]
    public string? PhysicalResourceId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}