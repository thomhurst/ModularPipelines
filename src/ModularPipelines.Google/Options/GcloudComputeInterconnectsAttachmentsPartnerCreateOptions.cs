using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "attachments", "partner", "create")]
public record GcloudComputeInterconnectsAttachmentsPartnerCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--edge-availability-domain")] string EdgeAvailabilityDomain,
[property: CommandSwitch("--router")] string Router
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-admin")]
    public bool? EnableAdmin { get; set; }

    [CommandSwitch("--encryption")]
    public string? Encryption { get; set; }

    [CommandSwitch("--ipsec-internal-addresses")]
    public string[]? IpsecInternalAddresses { get; set; }

    [CommandSwitch("--mtu")]
    public string? Mtu { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}