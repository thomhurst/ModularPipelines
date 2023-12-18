using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("offure", "hyperv", "host", "show")]
public record AzOffazureHypervHostShowOptions : AzOptions
{
    [CommandSwitch("--host-name")]
    public string? HostName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--site-name")]
    public string? SiteName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}