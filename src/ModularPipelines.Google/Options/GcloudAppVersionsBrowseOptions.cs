using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "versions", "browse")]
public record GcloudAppVersionsBrowseOptions(
[property: PositionalArgument] string Versions
) : GcloudOptions
{
    [BooleanCommandSwitch("--launch-browser")]
    public bool? LaunchBrowser { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }
}