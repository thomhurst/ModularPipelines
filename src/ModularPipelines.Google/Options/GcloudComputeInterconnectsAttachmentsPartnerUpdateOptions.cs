using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "attachments", "partner", "update")]
public record GcloudComputeInterconnectsAttachmentsPartnerUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-admin")]
    public bool? EnableAdmin { get; set; }

    [CliOption("--mtu")]
    public string? Mtu { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}