using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "export")]
public record GcloudComputeInstancesExportOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}