using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "check-in-license")]
public record AwsLicenseManagerCheckInLicenseOptions(
[property: CliOption("--license-consumption-token")] string LicenseConsumptionToken
) : AwsOptions
{
    [CliOption("--beneficiary")]
    public string? Beneficiary { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}