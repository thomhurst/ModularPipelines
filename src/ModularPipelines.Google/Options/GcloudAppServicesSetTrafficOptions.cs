using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "services", "set-traffic")]
public record GcloudAppServicesSetTrafficOptions(
[property: PositionalArgument] string Services,
[property: CommandSwitch("--splits")] string[] Splits
) : GcloudOptions
{
    [BooleanCommandSwitch("--migrate")]
    public bool? Migrate { get; set; }

    [CommandSwitch("--split-by")]
    public string? SplitBy { get; set; }
}