using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "replicate-secret-to-regions")]
public record AwsSecretsmanagerReplicateSecretToRegionsOptions(
[property: CliOption("--secret-id")] string SecretId,
[property: CliOption("--add-replica-regions")] string[] AddReplicaRegions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}