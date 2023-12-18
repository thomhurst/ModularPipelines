using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automation", "runbook", "create")]
public record AzAutomationRunbookCreateOptions(
[property: CommandSwitch("--automation-account-name")] int AutomationAccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--log-activity-trace")]
    public string? LogActivityTrace { get; set; }

    [BooleanCommandSwitch("--log-progress")]
    public bool? LogProgress { get; set; }

    [BooleanCommandSwitch("--log-verbose")]
    public bool? LogVerbose { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}