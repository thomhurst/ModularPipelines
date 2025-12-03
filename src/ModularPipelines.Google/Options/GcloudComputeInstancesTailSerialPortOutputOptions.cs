using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "tail-serial-port-output")]
public record GcloudComputeInstancesTailSerialPortOutputOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}