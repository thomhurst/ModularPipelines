using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "versions", "describe")]
public record GcloudAppVersionsDescribeOptions : GcloudOptions
{
    public GcloudAppVersionsDescribeOptions(
        string version,
        string service
    )
    {
        GcloudAppVersionsDescribeOptionsVersion = version;
        Service = service;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudAppVersionsDescribeOptionsVersion { get; set; }

    [CliOption("--service")]
    public string Service { get; set; }
}
