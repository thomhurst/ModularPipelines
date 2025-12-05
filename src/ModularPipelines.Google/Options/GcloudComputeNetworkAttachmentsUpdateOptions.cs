using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-attachments", "update")]
public record GcloudComputeNetworkAttachmentsUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--producer-accept-list")]
    public string[]? ProducerAcceptList { get; set; }

    [CliOption("--producer-reject-list")]
    public string[]? ProducerRejectList { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--subnets")]
    public string[]? Subnets { get; set; }

    [CliOption("--subnets-region")]
    public string? SubnetsRegion { get; set; }
}