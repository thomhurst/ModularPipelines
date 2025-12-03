using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "describe-app-instance-admin")]
public record AwsChimeSdkIdentityDescribeAppInstanceAdminOptions(
[property: CliOption("--app-instance-admin-arn")] string AppInstanceAdminArn,
[property: CliOption("--app-instance-arn")] string AppInstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}