using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logicapp", "config", "appsettings", "set")]
public record AzLogicappConfigAppsettingsSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--slot-settings")]
    public string? SlotSettings { get; set; }
}