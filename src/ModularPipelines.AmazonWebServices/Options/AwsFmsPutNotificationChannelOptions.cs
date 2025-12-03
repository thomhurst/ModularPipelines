using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fms", "put-notification-channel")]
public record AwsFmsPutNotificationChannelOptions(
[property: CliOption("--sns-topic-arn")] string SnsTopicArn,
[property: CliOption("--sns-role-name")] string SnsRoleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}