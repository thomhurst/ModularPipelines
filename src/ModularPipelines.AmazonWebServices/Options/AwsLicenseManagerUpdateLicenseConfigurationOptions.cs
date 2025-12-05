using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "update-license-configuration")]
public record AwsLicenseManagerUpdateLicenseConfigurationOptions(
[property: CliOption("--license-configuration-arn")] string LicenseConfigurationArn
) : AwsOptions
{
    [CliOption("--license-configuration-status")]
    public string? LicenseConfigurationStatus { get; set; }

    [CliOption("--license-rules")]
    public string[]? LicenseRules { get; set; }

    [CliOption("--license-count")]
    public long? LicenseCount { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--product-information-list")]
    public string[]? ProductInformationList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}