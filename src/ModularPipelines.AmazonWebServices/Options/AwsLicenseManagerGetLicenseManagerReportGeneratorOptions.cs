using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "get-license-manager-report-generator")]
public record AwsLicenseManagerGetLicenseManagerReportGeneratorOptions(
[property: CliOption("--license-manager-report-generator-arn")] string LicenseManagerReportGeneratorArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}