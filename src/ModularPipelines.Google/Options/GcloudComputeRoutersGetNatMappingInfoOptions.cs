using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "get-nat-mapping-info")]
public record GcloudComputeRoutersGetNatMappingInfoOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--nat-name")]
    public string? NatName { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}