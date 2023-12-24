using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisvideo", "delete-stream")]
public record AwsKinesisvideoDeleteStreamOptions(
[property: CommandSwitch("--stream-arn")] string StreamArn
) : AwsOptions
{
    [CommandSwitch("--current-version")]
    public string? CurrentVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}