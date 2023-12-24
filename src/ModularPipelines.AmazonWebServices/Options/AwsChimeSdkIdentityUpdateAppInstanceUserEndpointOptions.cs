using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "update-app-instance-user-endpoint")]
public record AwsChimeSdkIdentityUpdateAppInstanceUserEndpointOptions(
[property: CommandSwitch("--app-instance-user-arn")] string AppInstanceUserArn,
[property: CommandSwitch("--endpoint-id")] string EndpointId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--allow-messages")]
    public string? AllowMessages { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}