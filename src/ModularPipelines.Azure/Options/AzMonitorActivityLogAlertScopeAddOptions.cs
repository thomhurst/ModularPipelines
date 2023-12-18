using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "activity-log", "alert", "scope", "add")]
public record AzMonitorActivityLogAlertScopeAddOptions(
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [CommandSwitch("--activity-log-alert-name")]
    public string? ActivityLogAlertName { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [BooleanCommandSwitch("--reset")]
    public bool? Reset { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}