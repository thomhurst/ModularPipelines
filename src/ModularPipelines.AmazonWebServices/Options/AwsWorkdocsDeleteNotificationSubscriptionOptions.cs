using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "delete-notification-subscription")]
public record AwsWorkdocsDeleteNotificationSubscriptionOptions(
[property: CommandSwitch("--subscription-id")] string SubscriptionId,
[property: CommandSwitch("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}