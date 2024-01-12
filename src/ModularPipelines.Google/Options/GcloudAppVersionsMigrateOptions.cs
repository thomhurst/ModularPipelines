using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "versions", "migrate")]
public record GcloudAppVersionsMigrateOptions : GcloudOptions
{
    public GcloudAppVersionsMigrateOptions(
        string version
    )
    {
        GcloudAppVersionsMigrateOptionsVersion = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudAppVersionsMigrateOptionsVersion { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }
}
