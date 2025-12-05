using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "delete-secret")]
public record AwsSecretsmanagerDeleteSecretOptions(
[property: CliOption("--secret-id")] string SecretId
) : AwsOptions
{
    [CliOption("--recovery-window-in-days")]
    public long? RecoveryWindowInDays { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}