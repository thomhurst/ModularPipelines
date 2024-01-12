using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "perimeters", "dry-run", "enforce")]
public record GcloudAccessContextManagerPerimetersDryRunEnforceOptions(
[property: PositionalArgument] string Perimeter,
[property: PositionalArgument] string Policy
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}