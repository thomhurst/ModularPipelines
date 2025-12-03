using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "create-token")]
public record AwsLicenseManagerCreateTokenOptions(
[property: CliOption("--license-arn")] string LicenseArn,
[property: CliOption("--client-token")] string ClientToken
) : AwsOptions
{
    [CliOption("--role-arns")]
    public string[]? RoleArns { get; set; }

    [CliOption("--expiration-in-days")]
    public int? ExpirationInDays { get; set; }

    [CliOption("--token-properties")]
    public string[]? TokenProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}