using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "instances", "describe")]
public record GcloudAppInstancesDescribeOptions : GcloudOptions
{
    public GcloudAppInstancesDescribeOptions(
        string instance,
        string service,
        string version
    )
    {
        Instance = instance;
        Service = service;
        Version = version;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Instance { get; set; }

    [CliOption("--service")]
    public string Service { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }
}
