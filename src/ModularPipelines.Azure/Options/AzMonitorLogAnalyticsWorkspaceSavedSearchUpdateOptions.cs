using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace", "saved-search", "update")]
public record AzMonitorLogAnalyticsWorkspaceSavedSearchUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--fa")]
    public string? Fa { get; set; }

    [CommandSwitch("--fp")]
    public string? Fp { get; set; }

    [CommandSwitch("--saved-query")]
    public string? SavedQuery { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

