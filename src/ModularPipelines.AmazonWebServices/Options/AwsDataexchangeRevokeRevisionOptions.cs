using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataexchange", "revoke-revision")]
public record AwsDataexchangeRevokeRevisionOptions(
[property: CliOption("--data-set-id")] string DataSetId,
[property: CliOption("--revision-id")] string RevisionId,
[property: CliOption("--revocation-comment")] string RevocationComment
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}