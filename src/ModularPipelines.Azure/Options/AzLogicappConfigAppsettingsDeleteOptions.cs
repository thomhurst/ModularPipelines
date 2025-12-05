using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("logicapp", "config", "appsettings", "delete")]
public record AzLogicappConfigAppsettingsDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--setting-names")] string SettingNames
) : AzOptions
{
    [CliOption("--slot")]
    public string? Slot { get; set; }
}