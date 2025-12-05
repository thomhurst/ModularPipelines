using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "create-license-manager-report-generator")]
public record AwsLicenseManagerCreateLicenseManagerReportGeneratorOptions(
[property: CliOption("--report-generator-name")] string ReportGeneratorName,
[property: CliOption("--type")] string[] Type,
[property: CliOption("--report-context")] string ReportContext,
[property: CliOption("--report-frequency")] string ReportFrequency,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}