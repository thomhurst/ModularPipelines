using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "extend-license-consumption")]
public record AwsLicenseManagerExtendLicenseConsumptionOptions(
[property: CliOption("--license-consumption-token")] string LicenseConsumptionToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}