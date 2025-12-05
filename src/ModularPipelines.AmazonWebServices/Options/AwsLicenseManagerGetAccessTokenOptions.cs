using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "get-access-token")]
public record AwsLicenseManagerGetAccessTokenOptions(
[property: CliOption("--token")] string Token
) : AwsOptions
{
    [CliOption("--token-properties")]
    public string[]? TokenProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}