using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "versions", "stop")]
public record GcloudAppVersionsStopOptions(
[property: PositionalArgument] string Versions
) : GcloudOptions
{
    [CommandSwitch("--service")]
    public string? Service { get; set; }
}