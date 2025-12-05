using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-notifications", "create-notification-rule")]
public record AwsCodestarNotificationsCreateNotificationRuleOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--event-type-ids")] string[] EventTypeIds,
[property: CliOption("--resource")] string Resource,
[property: CliOption("--targets")] string[] Targets,
[property: CliOption("--detail-type")] string DetailType
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}