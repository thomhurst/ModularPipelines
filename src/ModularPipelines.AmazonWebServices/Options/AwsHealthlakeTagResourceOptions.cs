using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthlake", "tag-resource")]
public record AwsHealthlakeTagResourceOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--tags")] string[] Tags
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}