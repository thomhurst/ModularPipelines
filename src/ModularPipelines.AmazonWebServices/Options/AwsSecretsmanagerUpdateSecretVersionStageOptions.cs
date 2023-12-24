using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secretsmanager", "update-secret-version-stage")]
public record AwsSecretsmanagerUpdateSecretVersionStageOptions(
[property: CommandSwitch("--secret-id")] string SecretId,
[property: CommandSwitch("--version-stage")] string VersionStage
) : AwsOptions
{
    [CommandSwitch("--remove-from-version-id")]
    public string? RemoveFromVersionId { get; set; }

    [CommandSwitch("--move-to-version-id")]
    public string? MoveToVersionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}