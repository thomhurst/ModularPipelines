using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "remove-regions-from-replication")]
public record AwsSecretsmanagerRemoveRegionsFromReplicationOptions(
[property: CliOption("--secret-id")] string SecretId,
[property: CliOption("--remove-replica-regions")] string[] RemoveReplicaRegions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}