using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace", "saved-search", "create")]
public record AzMonitorLogAnalyticsWorkspaceSavedSearchCreateOptions(
[property: CommandSwitch("--category")] string Category,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--saved-query")] string SavedQuery,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--fa")]
    public string? Fa { get; set; }

    [CommandSwitch("--fp")]
    public string? Fp { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}