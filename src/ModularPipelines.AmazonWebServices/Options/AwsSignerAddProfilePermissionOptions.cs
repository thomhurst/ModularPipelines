using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "add-profile-permission")]
public record AwsSignerAddProfilePermissionOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--action")] string Action,
[property: CliOption("--principal")] string Principal,
[property: CliOption("--statement-id")] string StatementId
) : AwsOptions
{
    [CliOption("--profile-version")]
    public string? ProfileVersion { get; set; }

    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}