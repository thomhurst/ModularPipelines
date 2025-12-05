using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "create-secret")]
public record AwsSecretsmanagerCreateSecretOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--secret-binary")]
    public string? SecretBinary { get; set; }

    [CliOption("--secret-string")]
    public string? SecretString { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--add-replica-regions")]
    public string[]? AddReplicaRegions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}