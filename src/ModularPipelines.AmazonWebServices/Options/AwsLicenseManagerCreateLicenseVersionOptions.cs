using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "create-license-version")]
public record AwsLicenseManagerCreateLicenseVersionOptions(
[property: CliOption("--license-arn")] string LicenseArn,
[property: CliOption("--license-name")] string LicenseName,
[property: CliOption("--product-name")] string ProductName,
[property: CliOption("--issuer")] string Issuer,
[property: CliOption("--home-region")] string HomeRegion,
[property: CliOption("--validity")] string Validity,
[property: CliOption("--entitlements")] string[] Entitlements,
[property: CliOption("--consumption-configuration")] string ConsumptionConfiguration,
[property: CliOption("--status")] string Status,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--license-metadata")]
    public string[]? LicenseMetadata { get; set; }

    [CliOption("--source-version")]
    public string? SourceVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}