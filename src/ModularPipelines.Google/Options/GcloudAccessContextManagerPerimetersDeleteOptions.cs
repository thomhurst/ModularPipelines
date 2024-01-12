using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "perimeters", "delete")]
public record GcloudAccessContextManagerPerimetersDeleteOptions(
[property: PositionalArgument] string Perimeter,
[property: PositionalArgument] string Policy
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}