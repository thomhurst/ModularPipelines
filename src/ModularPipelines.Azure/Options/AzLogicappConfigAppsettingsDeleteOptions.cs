using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logicapp", "config", "appsettings", "delete")]
public record AzLogicappConfigAppsettingsDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--setting-names")] string SettingNames
) : AzOptions
{
    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}