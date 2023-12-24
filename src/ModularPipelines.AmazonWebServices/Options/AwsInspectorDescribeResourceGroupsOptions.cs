using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector", "describe-resource-groups")]
public record AwsInspectorDescribeResourceGroupsOptions(
[property: CommandSwitch("--resource-group-arns")] string[] ResourceGroupArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}