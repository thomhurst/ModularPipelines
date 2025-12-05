using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "get-license-conversion-task")]
public record AwsLicenseManagerGetLicenseConversionTaskOptions(
[property: CliOption("--license-conversion-task-id")] string LicenseConversionTaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}