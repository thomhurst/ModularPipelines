using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "services", "browse")]
public record GcloudAppServicesBrowseOptions(
[property: PositionalArgument] string Services
) : GcloudOptions
{
    [BooleanCommandSwitch("--launch-browser")]
    public bool? LaunchBrowser { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}