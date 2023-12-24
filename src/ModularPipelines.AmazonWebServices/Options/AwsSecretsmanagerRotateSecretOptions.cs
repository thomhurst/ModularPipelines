using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secretsmanager", "rotate-secret")]
public record AwsSecretsmanagerRotateSecretOptions(
[property: CommandSwitch("--secret-id")] string SecretId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--rotation-lambda-arn")]
    public string? RotationLambdaArn { get; set; }

    [CommandSwitch("--rotation-rules")]
    public string? RotationRules { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}