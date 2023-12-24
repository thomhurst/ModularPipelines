using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "tag-resource")]
public record AwsCloudfrontTagResourceOptions(
[property: CommandSwitch("--resource")] string Resource,
[property: CommandSwitch("--tags")] string Tags
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}