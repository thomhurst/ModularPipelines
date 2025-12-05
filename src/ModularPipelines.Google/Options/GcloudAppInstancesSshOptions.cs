using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "instances", "ssh")]
public record GcloudAppInstancesSshOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Command
) : GcloudOptions
{
    [CliOption("--container")]
    public string? Container { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliFlag("--tunnel-through-iap")]
    public bool? TunnelThroughIap { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}