using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "get-serial-port-output")]
public record GcloudComputeInstancesGetSerialPortOutputOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--start")]
    public string? Start { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}