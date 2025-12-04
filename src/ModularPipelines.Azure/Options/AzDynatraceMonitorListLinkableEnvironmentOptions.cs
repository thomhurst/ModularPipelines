using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dynatrace", "monitor", "list-linkable-environment")]
public record AzDynatraceMonitorListLinkableEnvironmentOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliOption("--user-principal")]
    public string? UserPrincipal { get; set; }
}