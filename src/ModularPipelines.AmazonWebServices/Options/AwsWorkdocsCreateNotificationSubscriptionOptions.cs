using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "create-notification-subscription")]
public record AwsWorkdocsCreateNotificationSubscriptionOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--subscription-type")] string SubscriptionType,
[property: CliOption("--notification-endpoint")] string NotificationEndpoint
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}