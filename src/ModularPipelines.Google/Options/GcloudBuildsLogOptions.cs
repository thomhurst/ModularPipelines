using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "log")]
public record GcloudBuildsLogOptions(
[property: PositionalArgument] string Build
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [BooleanCommandSwitch("--stream")]
    public bool? Stream { get; set; }
}