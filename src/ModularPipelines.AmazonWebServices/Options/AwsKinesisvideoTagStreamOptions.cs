using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisvideo", "tag-stream")]
public record AwsKinesisvideoTagStreamOptions(
[property: CommandSwitch("--tags")] IEnumerable<KeyValue> Tags
) : AwsOptions
{
    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}