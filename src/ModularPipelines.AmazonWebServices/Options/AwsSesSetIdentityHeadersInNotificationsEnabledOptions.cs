using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "set-identity-headers-in-notifications-enabled")]
public record AwsSesSetIdentityHeadersInNotificationsEnabledOptions(
[property: CliOption("--identity")] string Identity,
[property: CliOption("--notification-type")] string NotificationType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}