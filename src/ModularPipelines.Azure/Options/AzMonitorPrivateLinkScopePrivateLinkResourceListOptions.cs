using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "private-link-scope", "private-link-resource", "list")]
public record AzMonitorPrivateLinkScopePrivateLinkResourceListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--scope-name")] string ScopeName
) : AzOptions
{
    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }
}