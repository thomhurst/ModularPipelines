using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "get-guest-attributes")]
public record GcloudComputeTpusTpuVmGetGuestAttributesOptions(
[property: CliArgument] string Tpu,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliOption("--query-path")]
    public string? QueryPath { get; set; }
}