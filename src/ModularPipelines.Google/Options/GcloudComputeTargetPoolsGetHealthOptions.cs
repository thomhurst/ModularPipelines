using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-pools", "get-health")]
public record GcloudComputeTargetPoolsGetHealthOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}