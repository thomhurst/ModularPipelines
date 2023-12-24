using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-notifications", "update-notification-rule")]
public record AwsCodestarNotificationsUpdateNotificationRuleOptions(
[property: CommandSwitch("--arn")] string Arn
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--event-type-ids")]
    public string[]? EventTypeIds { get; set; }

    [CommandSwitch("--targets")]
    public string[]? Targets { get; set; }

    [CommandSwitch("--detail-type")]
    public string? DetailType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}