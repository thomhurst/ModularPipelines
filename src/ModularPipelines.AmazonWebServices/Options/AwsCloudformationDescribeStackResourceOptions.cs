using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "describe-stack-resource")]
public record AwsCloudformationDescribeStackResourceOptions(
[property: CommandSwitch("--stack-name")] string StackName,
[property: CommandSwitch("--logical-resource-id")] string LogicalResourceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}