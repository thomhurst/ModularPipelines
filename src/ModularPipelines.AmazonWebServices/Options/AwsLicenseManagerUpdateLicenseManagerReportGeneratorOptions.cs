using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "update-license-manager-report-generator")]
public record AwsLicenseManagerUpdateLicenseManagerReportGeneratorOptions(
[property: CliOption("--license-manager-report-generator-arn")] string LicenseManagerReportGeneratorArn,
[property: CliOption("--report-generator-name")] string ReportGeneratorName,
[property: CliOption("--type")] string[] Type,
[property: CliOption("--report-context")] string ReportContext,
[property: CliOption("--report-frequency")] string ReportFrequency,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}