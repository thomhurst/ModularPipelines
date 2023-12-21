using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apic", "api", "deployment", "show")]
public record AzApicApiDeploymentShowOptions : AzOptions
{
    [CommandSwitch("--api")]
    public string? Api { get; set; }

    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}