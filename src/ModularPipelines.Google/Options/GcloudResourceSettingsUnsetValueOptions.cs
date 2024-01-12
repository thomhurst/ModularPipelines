using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-settings", "unset-value")]
public record GcloudResourceSettingsUnsetValueOptions : GcloudOptions
{
    public GcloudResourceSettingsUnsetValueOptions(
        string settingName,
        string folder,
        string organization,
        string project
    )
    {
        SettingName = settingName;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string SettingName { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
