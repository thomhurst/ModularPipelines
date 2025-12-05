using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-settings", "describe")]
public record GcloudResourceSettingsDescribeOptions : GcloudOptions
{
    public GcloudResourceSettingsDescribeOptions(
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string SettingName { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliFlag("--effective")]
    public bool? Effective { get; set; }
}
