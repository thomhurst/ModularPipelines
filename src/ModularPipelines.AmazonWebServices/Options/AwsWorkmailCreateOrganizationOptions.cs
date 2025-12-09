using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "create-organization")]
public record AwsWorkmailCreateOrganizationOptions(
[property: CliOption("--alias")] string Alias
) : AwsOptions
{
    [CliOption("--directory-id")]
    public string? DirectoryId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--domains")]
    public string[]? Domains { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}