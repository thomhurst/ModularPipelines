using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-streaming-distribution")]
public record AwsCloudfrontCreateStreamingDistributionOptions(
[property: CommandSwitch("--streaming-distribution-config")] string StreamingDistributionConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}