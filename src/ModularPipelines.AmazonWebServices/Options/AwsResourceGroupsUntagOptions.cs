using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-groups", "untag")]
public record AwsResourceGroupsUntagOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--keys")] string[] Keys
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}