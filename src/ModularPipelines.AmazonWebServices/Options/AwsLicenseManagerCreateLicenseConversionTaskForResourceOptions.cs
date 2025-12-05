using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "create-license-conversion-task-for-resource")]
public record AwsLicenseManagerCreateLicenseConversionTaskForResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--source-license-context")] string SourceLicenseContext,
[property: CliOption("--destination-license-context")] string DestinationLicenseContext
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}