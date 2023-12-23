using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "create-notification-subscription")]
public record AwsWorkdocsCreateNotificationSubscriptionOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--subscription-type")] string SubscriptionType,
[property: CommandSwitch("--notification-endpoint")] string NotificationEndpoint
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}