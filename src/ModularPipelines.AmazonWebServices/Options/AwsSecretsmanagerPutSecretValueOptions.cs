using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "put-secret-value")]
public record AwsSecretsmanagerPutSecretValueOptions(
[property: CliOption("--secret-id")] string SecretId
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--secret-binary")]
    public string? SecretBinary { get; set; }

    [CliOption("--secret-string")]
    public string? SecretString { get; set; }

    [CliOption("--version-stages")]
    public string[]? VersionStages { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}