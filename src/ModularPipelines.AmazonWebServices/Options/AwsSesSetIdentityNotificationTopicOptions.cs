using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "set-identity-notification-topic")]
public record AwsSesSetIdentityNotificationTopicOptions(
[property: CliOption("--identity")] string Identity,
[property: CliOption("--notification-type")] string NotificationType
) : AwsOptions
{
    [CliOption("--sns-topic")]
    public string? SnsTopic { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}