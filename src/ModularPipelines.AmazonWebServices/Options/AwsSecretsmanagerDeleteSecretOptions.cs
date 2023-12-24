using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secretsmanager", "delete-secret")]
public record AwsSecretsmanagerDeleteSecretOptions(
[property: CommandSwitch("--secret-id")] string SecretId
) : AwsOptions
{
    [CommandSwitch("--recovery-window-in-days")]
    public long? RecoveryWindowInDays { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}