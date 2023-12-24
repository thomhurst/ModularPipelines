using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisvideo", "update-image-generation-configuration")]
public record AwsKinesisvideoUpdateImageGenerationConfigurationOptions : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--image-generation-configuration")]
    public string? ImageGenerationConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}