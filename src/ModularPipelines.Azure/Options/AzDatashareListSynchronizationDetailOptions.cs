using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datashare", "list-synchronization-detail")]
public record AzDatashareListSynchronizationDetailOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--consumer-email")]
    public string? ConsumerEmail { get; set; }

    [CliOption("--consumer-name")]
    public string? ConsumerName { get; set; }

    [CliOption("--consumer-tenant-name")]
    public string? ConsumerTenantName { get; set; }

    [CliOption("--duration-ms")]
    public string? DurationMs { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--synchronization-id")]
    public string? SynchronizationId { get; set; }
}