using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "schedule", "create")]
public record AzDevcenterAdminScheduleCreateOptions(
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--project")] string Project,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--time")] string Time,
[property: CommandSwitch("--time-zone")] string TimeZone
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }
}