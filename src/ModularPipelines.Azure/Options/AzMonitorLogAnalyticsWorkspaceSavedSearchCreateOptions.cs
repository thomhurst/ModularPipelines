using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "workspace", "saved-search", "create")]
public record AzMonitorLogAnalyticsWorkspaceSavedSearchCreateOptions(
[property: CliOption("--category")] string Category,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--saved-query")] string SavedQuery,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--fa")]
    public string? Fa { get; set; }

    [CliOption("--fp")]
    public string? Fp { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}