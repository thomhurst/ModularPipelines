using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "versions", "describe")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudAppVersionsDescribeOptionsVersion { get; set; }

    [CommandSwitch("--service")]
    public string Service { get; set; }
}
