using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "perimeters", "dry-run", "enforce")]
public record GcloudAccessContextManagerPerimetersDryRunEnforceOptions(
[property: CliArgument] string Perimeter,
[property: CliArgument] string Policy
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}