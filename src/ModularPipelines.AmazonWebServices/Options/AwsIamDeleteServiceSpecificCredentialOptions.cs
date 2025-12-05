using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "delete-service-specific-credential")]
public record AwsIamDeleteServiceSpecificCredentialOptions(
[property: CliOption("--service-specific-credential-id")] string ServiceSpecificCredentialId
) : AwsOptions
{
    [CliOption("--user-name")]
    public string? UserName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}