using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "update-secret-version-stage")]
public record AwsSecretsmanagerUpdateSecretVersionStageOptions(
[property: CliOption("--secret-id")] string SecretId,
[property: CliOption("--version-stage")] string VersionStage
) : AwsOptions
{
    [CliOption("--remove-from-version-id")]
    public string? RemoveFromVersionId { get; set; }

    [CliOption("--move-to-version-id")]
    public string? MoveToVersionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}