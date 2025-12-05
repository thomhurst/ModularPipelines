using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "rotate-secret")]
public record AwsSecretsmanagerRotateSecretOptions(
[property: CliOption("--secret-id")] string SecretId
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--rotation-lambda-arn")]
    public string? RotationLambdaArn { get; set; }

    [CliOption("--rotation-rules")]
    public string? RotationRules { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}