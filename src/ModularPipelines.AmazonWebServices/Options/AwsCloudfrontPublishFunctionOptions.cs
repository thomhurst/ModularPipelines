using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "publish-function")]
public record AwsCloudfrontPublishFunctionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--if-match")] string IfMatch
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}