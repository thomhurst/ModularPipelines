using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "get-license-configuration")]
public record AwsLicenseManagerGetLicenseConfigurationOptions(
[property: CliOption("--license-configuration-arn")] string LicenseConfigurationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}