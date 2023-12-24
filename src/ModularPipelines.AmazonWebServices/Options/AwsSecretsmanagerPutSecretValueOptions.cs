using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secretsmanager", "put-secret-value")]
public record AwsSecretsmanagerPutSecretValueOptions(
[property: CommandSwitch("--secret-id")] string SecretId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--secret-binary")]
    public string? SecretBinary { get; set; }

    [CommandSwitch("--secret-string")]
    public string? SecretString { get; set; }

    [CommandSwitch("--version-stages")]
    public string[]? VersionStages { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}