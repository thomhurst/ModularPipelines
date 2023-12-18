using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "appsettings", "set")]
public record AzStaticwebappAppsettingsSetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--setting-names")] string SettingNames
) : AzOptions
{
    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}