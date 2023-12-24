using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "get-license-manager-report-generator")]
public record AwsLicenseManagerGetLicenseManagerReportGeneratorOptions(
[property: CommandSwitch("--license-manager-report-generator-arn")] string LicenseManagerReportGeneratorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}