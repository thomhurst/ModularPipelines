using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "update-service-specific-credential")]
public record AwsIamUpdateServiceSpecificCredentialOptions(
[property: CliOption("--service-specific-credential-id")] string ServiceSpecificCredentialId,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--user-name")]
    public string? UserName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}