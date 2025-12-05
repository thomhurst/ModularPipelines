using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "delete-app-instance-user")]
public record AwsChimeSdkIdentityDeleteAppInstanceUserOptions(
[property: CliOption("--app-instance-user-arn")] string AppInstanceUserArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}