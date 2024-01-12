using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "http-health-checks", "list")]
public record GcloudComputeHttpHealthChecksListOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }
}