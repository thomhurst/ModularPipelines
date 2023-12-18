using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "list-synchronization-detail")]
public record AzDatashareListSynchronizationDetailOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--consumer-email")]
    public string? ConsumerEmail { get; set; }

    [CommandSwitch("--consumer-name")]
    public string? ConsumerName { get; set; }

    [CommandSwitch("--consumer-tenant-name")]
    public string? ConsumerTenantName { get; set; }

    [CommandSwitch("--duration-ms")]
    public string? DurationMs { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--orderby")]
    public string? Orderby { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--synchronization-id")]
    public string? SynchronizationId { get; set; }
}

