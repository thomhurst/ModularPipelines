using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "threat-indicator", "list")]
public record AzSentinelThreatIndicatorListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}