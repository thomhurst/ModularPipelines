using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "update-license-manager-report-generator")]
public record AwsLicenseManagerUpdateLicenseManagerReportGeneratorOptions(
[property: CommandSwitch("--license-manager-report-generator-arn")] string LicenseManagerReportGeneratorArn,
[property: CommandSwitch("--report-generator-name")] string ReportGeneratorName,
[property: CommandSwitch("--type")] string[] Type,
[property: CommandSwitch("--report-context")] string ReportContext,
[property: CommandSwitch("--report-frequency")] string ReportFrequency,
[property: CommandSwitch("--client-token")] string ClientToken
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}