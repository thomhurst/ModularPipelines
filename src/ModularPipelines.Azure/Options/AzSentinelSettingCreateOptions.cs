using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "setting", "create")]
public record AzSentinelSettingCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--entity-analytics")]
    public string? EntityAnalytics { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--ueba")]
    public string? Ueba { get; set; }
}