using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "describe-app-instance-admin")]
public record AwsChimeSdkIdentityDescribeAppInstanceAdminOptions(
[property: CommandSwitch("--app-instance-admin-arn")] string AppInstanceAdminArn,
[property: CommandSwitch("--app-instance-arn")] string AppInstanceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}