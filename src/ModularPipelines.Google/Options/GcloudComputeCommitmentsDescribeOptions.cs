using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "commitments", "describe")]
public record GcloudComputeCommitmentsDescribeOptions(
[property: CliArgument] string Commitment
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}