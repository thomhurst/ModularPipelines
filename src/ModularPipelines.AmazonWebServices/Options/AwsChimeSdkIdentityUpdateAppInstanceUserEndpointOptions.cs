using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "update-app-instance-user-endpoint")]
public record AwsChimeSdkIdentityUpdateAppInstanceUserEndpointOptions(
[property: CliOption("--app-instance-user-arn")] string AppInstanceUserArn,
[property: CliOption("--endpoint-id")] string EndpointId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--allow-messages")]
    public string? AllowMessages { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}