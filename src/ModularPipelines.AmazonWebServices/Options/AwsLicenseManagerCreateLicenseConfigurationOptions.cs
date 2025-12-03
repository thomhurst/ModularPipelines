using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "create-license-configuration")]
public record AwsLicenseManagerCreateLicenseConfigurationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--license-counting-type")] string LicenseCountingType,
[property: CliOption("--license-count")] long LicenseCount
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--license-rules")]
    public string[]? LicenseRules { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--product-information-list")]
    public string[]? ProductInformationList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}