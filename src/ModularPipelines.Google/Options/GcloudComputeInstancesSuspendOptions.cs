using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "suspend")]
public record GcloudComputeInstancesSuspendOptions(
[property: CliArgument] string InstanceNames
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--discard-local-ssd[")]
    public string[]? DiscardLocalSsd { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}