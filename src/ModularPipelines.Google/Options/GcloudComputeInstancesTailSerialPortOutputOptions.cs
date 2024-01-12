using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "tail-serial-port-output")]
public record GcloudComputeInstancesTailSerialPortOutputOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}