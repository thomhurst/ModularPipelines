using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-groups", "group-resources")]
public record AwsResourceGroupsGroupResourcesOptions(
[property: CommandSwitch("--group")] string Group,
[property: CommandSwitch("--resource-arns")] string[] ResourceArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}