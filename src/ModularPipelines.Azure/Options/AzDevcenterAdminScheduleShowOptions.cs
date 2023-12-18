using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "schedule", "show")]
public record AzDevcenterAdminScheduleShowOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--pool-name")]
    public string? PoolName { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}