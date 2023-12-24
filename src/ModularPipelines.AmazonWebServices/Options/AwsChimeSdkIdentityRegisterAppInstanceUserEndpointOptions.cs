using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "register-app-instance-user-endpoint")]
public record AwsChimeSdkIdentityRegisterAppInstanceUserEndpointOptions(
[property: CommandSwitch("--app-instance-user-arn")] string AppInstanceUserArn,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--endpoint-attributes")] string EndpointAttributes
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--allow-messages")]
    public string? AllowMessages { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}