using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-incidents", "update-response-plan")]
public record AwsSsmIncidentsUpdateResponsePlanOptions(
[property: CommandSwitch("--arn")] string Arn
) : AwsOptions
{
    [CommandSwitch("--actions")]
    public string[]? Actions { get; set; }

    [CommandSwitch("--chat-channel")]
    public string? ChatChannel { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--engagements")]
    public string[]? Engagements { get; set; }

    [CommandSwitch("--incident-template-dedupe-string")]
    public string? IncidentTemplateDedupeString { get; set; }

    [CommandSwitch("--incident-template-impact")]
    public int? IncidentTemplateImpact { get; set; }

    [CommandSwitch("--incident-template-notification-targets")]
    public string[]? IncidentTemplateNotificationTargets { get; set; }

    [CommandSwitch("--incident-template-summary")]
    public string? IncidentTemplateSummary { get; set; }

    [CommandSwitch("--incident-template-tags")]
    public IEnumerable<KeyValue>? IncidentTemplateTags { get; set; }

    [CommandSwitch("--incident-template-title")]
    public string? IncidentTemplateTitle { get; set; }

    [CommandSwitch("--integrations")]
    public string[]? Integrations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}