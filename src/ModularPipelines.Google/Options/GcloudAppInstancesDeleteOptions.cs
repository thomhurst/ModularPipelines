using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "instances", "delete")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Instance { get; set; }

    [CommandSwitch("--service")]
    public string Service { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }
}
