using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "subnets", "list")]
public record GcloudComputeNetworksSubnetsListOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }

    [CommandSwitch("--regions")]
    public string[]? Regions { get; set; }
}