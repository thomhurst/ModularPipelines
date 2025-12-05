using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "put-app-instance-retention-settings")]
public record AwsChimeSdkIdentityPutAppInstanceRetentionSettingsOptions(
[property: CliOption("--app-instance-arn")] string AppInstanceArn,
[property: CliOption("--app-instance-retention-settings")] string AppInstanceRetentionSettings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}