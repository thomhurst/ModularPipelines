using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "delete-license")]
public record AwsLicenseManagerDeleteLicenseOptions(
[property: CliOption("--license-arn")] string LicenseArn,
[property: CliOption("--source-version")] string SourceVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}