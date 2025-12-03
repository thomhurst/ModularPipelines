using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "versions", "migrate")]
public record GcloudAppVersionsMigrateOptions : GcloudOptions
{
    public GcloudAppVersionsMigrateOptions(
        string version
    )
    {
        GcloudAppVersionsMigrateOptionsVersion = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudAppVersionsMigrateOptionsVersion { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }
}
