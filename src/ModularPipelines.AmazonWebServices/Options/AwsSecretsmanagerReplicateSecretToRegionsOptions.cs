using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secretsmanager", "replicate-secret-to-regions")]
public record AwsSecretsmanagerReplicateSecretToRegionsOptions(
[property: CommandSwitch("--secret-id")] string SecretId,
[property: CommandSwitch("--add-replica-regions")] string[] AddReplicaRegions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}