using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "describe-app-instance-user")]
public record AwsChimeSdkIdentityDescribeAppInstanceUserOptions(
[property: CliOption("--app-instance-user-arn")] string AppInstanceUserArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}