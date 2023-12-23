using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "deregister-app-instance-user-endpoint")]
public record AwsChimeSdkIdentityDeregisterAppInstanceUserEndpointOptions(
[property: CommandSwitch("--app-instance-user-arn")] string AppInstanceUserArn,
[property: CommandSwitch("--endpoint-id")] string EndpointId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}