using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "batch-start")]
public record AwsMedialiveBatchStartOptions : AwsOptions
{
    [CliOption("--channel-ids")]
    public string[]? ChannelIds { get; set; }

    [CliOption("--multiplex-ids")]
    public string[]? MultiplexIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}