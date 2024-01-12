using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "versions", "delete")]
public record GcloudAppVersionsDeleteOptions(
[property: PositionalArgument] string Versions
) : GcloudOptions
{
    [CommandSwitch("--service")]
    public string? Service { get; set; }
}