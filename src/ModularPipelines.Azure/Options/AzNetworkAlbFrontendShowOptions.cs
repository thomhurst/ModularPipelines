using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "alb", "frontend", "show")]
public record AzNetworkAlbFrontendShowOptions : AzOptions
{
    [CommandSwitch("--alb-name")]
    public string? AlbName { get; set; }

    [CommandSwitch("--frontend-name")]
    public string? FrontendName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}