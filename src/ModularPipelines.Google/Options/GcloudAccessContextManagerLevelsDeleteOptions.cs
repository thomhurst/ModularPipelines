using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "levels", "delete")]
public record GcloudAccessContextManagerLevelsDeleteOptions(
[property: PositionalArgument] string Level,
[property: PositionalArgument] string Policy
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}