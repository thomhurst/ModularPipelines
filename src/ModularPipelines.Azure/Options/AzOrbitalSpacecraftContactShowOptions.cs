using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("orbital", "spacecraft", "contact", "show")]
public record AzOrbitalSpacecraftContactShowOptions : AzOptions
{
    [CommandSwitch("--contact-name")]
    public string? ContactName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--spacecraft-name")]
    public string? SpacecraftName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}