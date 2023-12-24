using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "create-app-instance-user")]
public record AwsChimeSdkIdentityCreateAppInstanceUserOptions(
[property: CommandSwitch("--app-instance-arn")] string AppInstanceArn,
[property: CommandSwitch("--app-instance-user-id")] string AppInstanceUserId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--expiration-settings")]
    public string? ExpirationSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}