using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "attachments", "partner", "create")]
public record GcloudComputeInterconnectsAttachmentsPartnerCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--edge-availability-domain")] string EdgeAvailabilityDomain,
[property: CliOption("--router")] string Router
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-admin")]
    public bool? EnableAdmin { get; set; }

    [CliOption("--encryption")]
    public string? Encryption { get; set; }

    [CliOption("--ipsec-internal-addresses")]
    public string[]? IpsecInternalAddresses { get; set; }

    [CliOption("--mtu")]
    public string? Mtu { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}