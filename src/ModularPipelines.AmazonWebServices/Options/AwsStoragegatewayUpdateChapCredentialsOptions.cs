using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "update-chap-credentials")]
public record AwsStoragegatewayUpdateChapCredentialsOptions(
[property: CommandSwitch("--target-arn")] string TargetArn,
[property: CommandSwitch("--secret-to-authenticate-initiator")] string SecretToAuthenticateInitiator,
[property: CommandSwitch("--initiator-name")] string InitiatorName
) : AwsOptions
{
    [CommandSwitch("--secret-to-authenticate-target")]
    public string? SecretToAuthenticateTarget { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}