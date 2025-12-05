using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "remove-profile-permission")]
public record AwsSignerRemoveProfilePermissionOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--revision-id")] string RevisionId,
[property: CliOption("--statement-id")] string StatementId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}