using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "get-grant")]
public record AwsLicenseManagerGetGrantOptions(
[property: CliOption("--grant-arn")] string GrantArn
) : AwsOptions
{
    [CliOption("--grant-version")]
    public string? GrantVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}