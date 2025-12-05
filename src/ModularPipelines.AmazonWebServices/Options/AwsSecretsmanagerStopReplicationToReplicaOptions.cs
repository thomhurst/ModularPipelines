using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "stop-replication-to-replica")]
public record AwsSecretsmanagerStopReplicationToReplicaOptions(
[property: CliOption("--secret-id")] string SecretId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}