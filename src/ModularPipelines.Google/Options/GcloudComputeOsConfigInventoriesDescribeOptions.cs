using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "inventories", "describe")]
public record GcloudComputeOsConfigInventoriesDescribeOptions(
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--view")]
    public string? View { get; set; }
}