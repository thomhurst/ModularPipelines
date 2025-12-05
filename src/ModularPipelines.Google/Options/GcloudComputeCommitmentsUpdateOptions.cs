using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "commitments", "update")]
public record GcloudComputeCommitmentsUpdateOptions(
[property: CliArgument] string Commitment
) : GcloudOptions
{
    [CliFlag("--auto-renew")]
    public bool? AutoRenew { get; set; }

    [CliOption("--plan")]
    public string? Plan { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}