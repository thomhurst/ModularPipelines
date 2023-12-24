using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisvideo", "untag-stream")]
public record AwsKinesisvideoUntagStreamOptions(
[property: CommandSwitch("--tag-key-list")] string[] TagKeyList
) : AwsOptions
{
    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}