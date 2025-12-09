using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "delete-grant")]
public record AwsLicenseManagerDeleteGrantOptions(
[property: CliOption("--grant-arn")] string GrantArn,
[property: CliOption("--grant-version")] string GrantVersion
) : AwsOptions
{
    [CliOption("--status-reason")]
    public string? StatusReason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}