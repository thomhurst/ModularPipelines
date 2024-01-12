using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "attachments", "partner", "update")]
public record GcloudComputeInterconnectsAttachmentsPartnerUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-admin")]
    public bool? EnableAdmin { get; set; }

    [CommandSwitch("--mtu")]
    public string? Mtu { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}