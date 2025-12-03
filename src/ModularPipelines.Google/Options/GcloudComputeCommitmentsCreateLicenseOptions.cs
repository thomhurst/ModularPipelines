using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "commitments", "create-license")]
public record GcloudComputeCommitmentsCreateLicenseOptions(
[property: CliArgument] string Commitment,
[property: CliOption("--amount")] string Amount,
[property: CliOption("--license")] string License,
[property: CliOption("--plan")] string Plan
) : GcloudOptions
{
    [CliOption("--cores-per-license")]
    public string? CoresPerLicense { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}