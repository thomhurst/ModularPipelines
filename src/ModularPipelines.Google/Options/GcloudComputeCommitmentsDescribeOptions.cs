using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "commitments", "describe")]
public record GcloudComputeCommitmentsDescribeOptions(
[property: PositionalArgument] string Commitment
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}