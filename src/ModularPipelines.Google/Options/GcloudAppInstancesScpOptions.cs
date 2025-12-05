using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "instances", "scp")]
public record GcloudAppInstancesScpOptions(
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliFlag("--compress")]
    public bool? Compress { get; set; }

    [CliFlag("--recurse")]
    public bool? Recurse { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliFlag("--tunnel-through-iap")]
    public bool? TunnelThroughIap { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}