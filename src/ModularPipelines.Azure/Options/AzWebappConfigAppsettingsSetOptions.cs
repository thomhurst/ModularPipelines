using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "appsettings", "set")]
public record AzWebappConfigAppsettingsSetOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--slot-settings")]
    public string? SlotSettings { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}