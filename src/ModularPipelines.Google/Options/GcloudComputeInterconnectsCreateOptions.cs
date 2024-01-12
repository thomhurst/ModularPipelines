using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "create")]
public record GcloudComputeInterconnectsCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--interconnect-type")] string InterconnectType,
[property: CommandSwitch("--link-type")] string LinkType,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--requested-link-count")] string RequestedLinkCount
) : GcloudOptions
{
    [BooleanCommandSwitch("--admin-enabled")]
    public bool? AdminEnabled { get; set; }

    [CommandSwitch("--customer-name")]
    public string? CustomerName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--noc-contact-email")]
    public string? NocContactEmail { get; set; }

    [CommandSwitch("--remote-location")]
    public string? RemoteLocation { get; set; }

    [CommandSwitch("--requested-features")]
    public string[]? RequestedFeatures { get; set; }
}