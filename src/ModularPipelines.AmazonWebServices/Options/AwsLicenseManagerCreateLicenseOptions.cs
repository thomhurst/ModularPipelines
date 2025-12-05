using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "create-license")]
public record AwsLicenseManagerCreateLicenseOptions(
[property: CliOption("--license-name")] string LicenseName,
[property: CliOption("--product-name")] string ProductName,
[property: CliOption("--product-sku")] string ProductSku,
[property: CliOption("--issuer")] string Issuer,
[property: CliOption("--home-region")] string HomeRegion,
[property: CliOption("--validity")] string Validity,
[property: CliOption("--entitlements")] string[] Entitlements,
[property: CliOption("--beneficiary")] string Beneficiary,
[property: CliOption("--consumption-configuration")] string ConsumptionConfiguration,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--license-metadata")]
    public string[]? LicenseMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}