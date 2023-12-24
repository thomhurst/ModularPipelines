using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-notifications", "create-notification-rule")]
public record AwsCodestarNotificationsCreateNotificationRuleOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--event-type-ids")] string[] EventTypeIds,
[property: CommandSwitch("--resource")] string Resource,
[property: CommandSwitch("--targets")] string[] Targets,
[property: CommandSwitch("--detail-type")] string DetailType
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}