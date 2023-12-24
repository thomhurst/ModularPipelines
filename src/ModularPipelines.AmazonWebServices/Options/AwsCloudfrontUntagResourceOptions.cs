using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "untag-resource")]
public record AwsCloudfrontUntagResourceOptions(
[property: CommandSwitch("--resource")] string Resource,
[property: CommandSwitch("--tag-keys")] string TagKeys
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}