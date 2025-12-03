using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "get-app-instance-retention-settings")]
public record AwsChimeSdkIdentityGetAppInstanceRetentionSettingsOptions(
[property: CliOption("--app-instance-arn")] string AppInstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}