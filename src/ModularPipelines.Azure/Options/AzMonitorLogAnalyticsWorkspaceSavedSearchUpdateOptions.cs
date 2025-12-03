using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "workspace", "saved-search", "update")]
public record AzMonitorLogAnalyticsWorkspaceSavedSearchUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--fa")]
    public string? Fa { get; set; }

    [CliOption("--fp")]
    public string? Fp { get; set; }

    [CliOption("--saved-query")]
    public string? SavedQuery { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}