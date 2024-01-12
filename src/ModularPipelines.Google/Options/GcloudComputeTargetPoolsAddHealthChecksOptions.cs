using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-pools", "add-health-checks")]
public record GcloudComputeTargetPoolsAddHealthChecksOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--http-health-check")] string HttpHealthCheck
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}