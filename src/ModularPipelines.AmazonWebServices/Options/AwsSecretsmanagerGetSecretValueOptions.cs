using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secretsmanager", "get-secret-value")]
public record AwsSecretsmanagerGetSecretValueOptions(
[property: CommandSwitch("--secret-id")] string SecretId
) : AwsOptions
{
    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--version-stage")]
    public string? VersionStage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}