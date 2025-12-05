using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("looker", "instances", "delete")]
public record GcloudLookerInstancesDeleteOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}