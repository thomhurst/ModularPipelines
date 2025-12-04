using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "config", "appsettings", "set")]
public record AzWebappConfigAppsettingsSetOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--slot-settings")]
    public string? SlotSettings { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}