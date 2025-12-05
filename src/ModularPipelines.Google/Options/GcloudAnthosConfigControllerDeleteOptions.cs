using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("anthos", "config", "controller", "delete")]
public record GcloudAnthosConfigControllerDeleteOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}