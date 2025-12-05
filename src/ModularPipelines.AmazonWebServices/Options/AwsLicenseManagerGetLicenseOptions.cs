using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "get-license")]
public record AwsLicenseManagerGetLicenseOptions(
[property: CliOption("--license-arn")] string LicenseArn
) : AwsOptions
{
    [CliOption("--license-version")]
    public string? LicenseVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}