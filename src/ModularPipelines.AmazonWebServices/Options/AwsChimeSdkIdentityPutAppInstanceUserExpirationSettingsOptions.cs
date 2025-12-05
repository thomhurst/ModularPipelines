using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "put-app-instance-user-expiration-settings")]
public record AwsChimeSdkIdentityPutAppInstanceUserExpirationSettingsOptions(
[property: CliOption("--app-instance-user-arn")] string AppInstanceUserArn
) : AwsOptions
{
    [CliOption("--expiration-settings")]
    public string? ExpirationSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}