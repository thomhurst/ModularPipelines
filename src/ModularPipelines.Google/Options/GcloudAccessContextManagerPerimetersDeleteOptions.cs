using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "delete")]
public record GcloudAccessContextManagerPerimetersDeleteOptions(
[property: CliArgument] string Perimeter,
[property: CliArgument] string Policy
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}