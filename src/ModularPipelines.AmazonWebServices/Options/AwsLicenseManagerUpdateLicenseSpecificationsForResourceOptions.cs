using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "update-license-specifications-for-resource")]
public record AwsLicenseManagerUpdateLicenseSpecificationsForResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CliOption("--add-license-specifications")]
    public string[]? AddLicenseSpecifications { get; set; }

    [CliOption("--remove-license-specifications")]
    public string[]? RemoveLicenseSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}