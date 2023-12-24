using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "set-identity-headers-in-notifications-enabled")]
public record AwsSesSetIdentityHeadersInNotificationsEnabledOptions(
[property: CommandSwitch("--identity")] string Identity,
[property: CommandSwitch("--notification-type")] string NotificationType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}