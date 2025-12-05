using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "list-grants")]
public record AwsKmsListGrantsOptions(
[property: CliOption("--key-id")] string KeyId
) : AwsOptions
{
    [CliOption("--grant-id")]
    public string? GrantId { get; set; }

    [CliOption("--grantee-principal")]
    public string? GranteePrincipal { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}