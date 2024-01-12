using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-attachments", "update")]
public record GcloudComputeNetworkAttachmentsUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--producer-accept-list")]
    public string[]? ProducerAcceptList { get; set; }

    [CommandSwitch("--producer-reject-list")]
    public string[]? ProducerRejectList { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--subnets")]
    public string[]? Subnets { get; set; }

    [CommandSwitch("--subnets-region")]
    public string? SubnetsRegion { get; set; }
}