using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emulators", "spanner", "start")]
public record GcloudEmulatorsSpannerStartOptions : GcloudOptions
{
    [CommandSwitch("--enable-fault-injection")]
    public string? EnableFaultInjection { get; set; }

    [CommandSwitch("--host-port")]
    public string? HostPort { get; set; }

    [CommandSwitch("--rest-port")]
    public string? RestPort { get; set; }

    [CommandSwitch("--use-docker")]
    public string? UseDocker { get; set; }
}