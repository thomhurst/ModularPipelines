using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "forwarding-rules", "export")]
public record GcloudComputeForwardingRulesExportOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}