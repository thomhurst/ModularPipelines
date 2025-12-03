using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emulators", "spanner", "start")]
public record GcloudEmulatorsSpannerStartOptions : GcloudOptions
{
    [CliOption("--enable-fault-injection")]
    public string? EnableFaultInjection { get; set; }

    [CliOption("--host-port")]
    public string? HostPort { get; set; }

    [CliOption("--rest-port")]
    public string? RestPort { get; set; }

    [CliOption("--use-docker")]
    public string? UseDocker { get; set; }
}