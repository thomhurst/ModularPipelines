using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secretsmanager", "remove-regions-from-replication")]
public record AwsSecretsmanagerRemoveRegionsFromReplicationOptions(
[property: CommandSwitch("--secret-id")] string SecretId,
[property: CommandSwitch("--remove-replica-regions")] string[] RemoveReplicaRegions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}