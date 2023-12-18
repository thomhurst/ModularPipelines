using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "action-group", "test-notifications", "create")]
public record AzMonitorActionGroupTestNotificationsCreateOptions(
[property: CommandSwitch("--action-group")] string ActionGroup,
[property: CommandSwitch("--alert-type")] string AlertType,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add-action")]
    public string? AddAction { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}