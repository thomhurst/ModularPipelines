using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "analytics-setting", "create")]
public record AzSentinelAnalyticsSettingCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--anomaly")]
    public string? Anomaly { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }
}