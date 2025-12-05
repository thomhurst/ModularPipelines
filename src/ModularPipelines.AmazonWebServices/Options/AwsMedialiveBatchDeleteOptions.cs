using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "batch-delete")]
public record AwsMedialiveBatchDeleteOptions : AwsOptions
{
    [CliOption("--channel-ids")]
    public string[]? ChannelIds { get; set; }

    [CliOption("--input-ids")]
    public string[]? InputIds { get; set; }

    [CliOption("--input-security-group-ids")]
    public string[]? InputSecurityGroupIds { get; set; }

    [CliOption("--multiplex-ids")]
    public string[]? MultiplexIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}