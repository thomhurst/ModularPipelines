using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-chap-credentials")]
public record AwsStoragegatewayUpdateChapCredentialsOptions(
[property: CliOption("--target-arn")] string TargetArn,
[property: CliOption("--secret-to-authenticate-initiator")] string SecretToAuthenticateInitiator,
[property: CliOption("--initiator-name")] string InitiatorName
) : AwsOptions
{
    [CliOption("--secret-to-authenticate-target")]
    public string? SecretToAuthenticateTarget { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}