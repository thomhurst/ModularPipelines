using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "batch-stop")]
public record AwsMedialiveBatchStopOptions : AwsOptions
{
    [CliOption("--channel-ids")]
    public string[]? ChannelIds { get; set; }

    [CliOption("--multiplex-ids")]
    public string[]? MultiplexIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}