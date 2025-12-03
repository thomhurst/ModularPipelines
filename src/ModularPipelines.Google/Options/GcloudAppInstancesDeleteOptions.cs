using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "instances", "delete")]
public record GcloudAppInstancesDeleteOptions : GcloudOptions
{
    public GcloudAppInstancesDeleteOptions(
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
