using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "batch-start")]
public record AwsMedialiveBatchStartOptions : AwsOptions
{
    [CommandSwitch("--channel-ids")]
    public string[]? ChannelIds { get; set; }

    [CommandSwitch("--multiplex-ids")]
    public string[]? MultiplexIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}