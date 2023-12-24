using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "batch-delete")]
public record AwsMedialiveBatchDeleteOptions : AwsOptions
{
    [CommandSwitch("--channel-ids")]
    public string[]? ChannelIds { get; set; }

    [CommandSwitch("--input-ids")]
    public string[]? InputIds { get; set; }

    [CommandSwitch("--input-security-group-ids")]
    public string[]? InputSecurityGroupIds { get; set; }

    [CommandSwitch("--multiplex-ids")]
    public string[]? MultiplexIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}