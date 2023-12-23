using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataexchange", "revoke-revision")]
public record AwsDataexchangeRevokeRevisionOptions(
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--revision-id")] string RevisionId,
[property: CommandSwitch("--revocation-comment")] string RevocationComment
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}