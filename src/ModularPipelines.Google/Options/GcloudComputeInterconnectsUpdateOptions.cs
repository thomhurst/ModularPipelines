using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "update")]
public record GcloudComputeInterconnectsUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--admin-enabled")]
    public bool? AdminEnabled { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--noc-contact-email")]
    public string? NocContactEmail { get; set; }

    [CliOption("--requested-link-count")]
    public string? RequestedLinkCount { get; set; }
}