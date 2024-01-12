using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "get-serial-port-output")]
public record GcloudComputeInstancesGetSerialPortOutputOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--start")]
    public string? Start { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}