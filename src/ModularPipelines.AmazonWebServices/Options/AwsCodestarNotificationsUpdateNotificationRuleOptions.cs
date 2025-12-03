using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-notifications", "update-notification-rule")]
public record AwsCodestarNotificationsUpdateNotificationRuleOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--event-type-ids")]
    public string[]? EventTypeIds { get; set; }

    [CliOption("--targets")]
    public string[]? Targets { get; set; }

    [CliOption("--detail-type")]
    public string? DetailType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}