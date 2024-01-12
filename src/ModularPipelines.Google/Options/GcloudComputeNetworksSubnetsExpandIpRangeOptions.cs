using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "subnets", "expand-ip-range")]
public record GcloudComputeNetworksSubnetsExpandIpRangeOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--prefix-length")] string PrefixLength
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}