using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "inventories", "describe")]
public record GcloudComputeOsConfigInventoriesDescribeOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--view")]
    public string? View { get; set; }
}