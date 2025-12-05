using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "describe-app-instance-user-endpoint")]
public record AwsChimeSdkIdentityDescribeAppInstanceUserEndpointOptions(
[property: CliOption("--app-instance-user-arn")] string AppInstanceUserArn,
[property: CliOption("--endpoint-id")] string EndpointId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}