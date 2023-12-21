using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedmachine", "private-link-resource", "show")]
public record AzConnectedmachinePrivateLinkResourceShowOptions : AzOptions
{
    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scope-name")]
    public string? ScopeName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}