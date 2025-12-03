using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-attachments", "create")]
public record GcloudComputeNetworkAttachmentsCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--subnets")] string[] Subnets
) : GcloudOptions
{
    [CliOption("--connection-preference")]
    public string? ConnectionPreference { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--producer-accept-list")]
    public string[]? ProducerAcceptList { get; set; }

    [CliOption("--producer-reject-list")]
    public string[]? ProducerRejectList { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--subnets-region")]
    public string? SubnetsRegion { get; set; }
}