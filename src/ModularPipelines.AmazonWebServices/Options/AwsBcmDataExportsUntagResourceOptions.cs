using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bcm-data-exports", "untag-resource")]
public record AwsBcmDataExportsUntagResourceOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--resource-tag-keys")] string[] ResourceTagKeys
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}