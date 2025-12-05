using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "create")]
public record GcloudComputeInterconnectsCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--interconnect-type")] string InterconnectType,
[property: CliOption("--link-type")] string LinkType,
[property: CliOption("--location")] string Location,
[property: CliOption("--requested-link-count")] string RequestedLinkCount
) : GcloudOptions
{
    [CliFlag("--admin-enabled")]
    public bool? AdminEnabled { get; set; }

    [CliOption("--customer-name")]
    public string? CustomerName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--noc-contact-email")]
    public string? NocContactEmail { get; set; }

    [CliOption("--remote-location")]
    public string? RemoteLocation { get; set; }

    [CliOption("--requested-features")]
    public string[]? RequestedFeatures { get; set; }
}