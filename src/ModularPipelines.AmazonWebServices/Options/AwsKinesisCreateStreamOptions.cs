using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis", "create-stream")]
public record AwsKinesisCreateStreamOptions(
[property: CommandSwitch("--stream-name")] string StreamName
) : AwsOptions
{
    [CommandSwitch("--shard-count")]
    public int? ShardCount { get; set; }

    [CommandSwitch("--stream-mode-details")]
    public string? StreamModeDetails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}