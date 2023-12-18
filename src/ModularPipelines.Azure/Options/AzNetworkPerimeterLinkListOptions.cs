using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "perimeter", "link", "list")]
public record AzNetworkPerimeterLinkListOptions(
[property: CommandSwitch("--perimeter-name")] string PerimeterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}