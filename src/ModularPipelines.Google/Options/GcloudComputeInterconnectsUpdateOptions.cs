using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "update")]
public record GcloudComputeInterconnectsUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [BooleanCommandSwitch("--admin-enabled")]
    public bool? AdminEnabled { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--noc-contact-email")]
    public string? NocContactEmail { get; set; }

    [CommandSwitch("--requested-link-count")]
    public string? RequestedLinkCount { get; set; }
}