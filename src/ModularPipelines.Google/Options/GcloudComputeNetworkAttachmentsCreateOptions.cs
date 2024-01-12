using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-attachments", "create")]
public record GcloudComputeNetworkAttachmentsCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--subnets")] string[] Subnets
) : GcloudOptions
{
    [CommandSwitch("--connection-preference")]
    public string? ConnectionPreference { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--producer-accept-list")]
    public string[]? ProducerAcceptList { get; set; }

    [CommandSwitch("--producer-reject-list")]
    public string[]? ProducerRejectList { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--subnets-region")]
    public string? SubnetsRegion { get; set; }
}