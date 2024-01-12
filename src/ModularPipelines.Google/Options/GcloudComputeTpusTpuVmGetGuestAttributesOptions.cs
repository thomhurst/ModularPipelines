using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "tpu-vm", "get-guest-attributes")]
public record GcloudComputeTpusTpuVmGetGuestAttributesOptions(
[property: PositionalArgument] string Tpu,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [CommandSwitch("--query-path")]
    public string? QueryPath { get; set; }
}