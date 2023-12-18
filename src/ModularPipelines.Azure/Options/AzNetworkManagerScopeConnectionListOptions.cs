using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "scope-connection", "list")]
public record AzNetworkManagerScopeConnectionListOptions(
[property: CommandSwitch("--network-manager")] string NetworkManager,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}