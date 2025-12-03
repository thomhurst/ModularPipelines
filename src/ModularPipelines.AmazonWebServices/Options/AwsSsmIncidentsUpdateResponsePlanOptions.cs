using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "update-response-plan")]
public record AwsSsmIncidentsUpdateResponsePlanOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--actions")]
    public string[]? Actions { get; set; }

    [CliOption("--chat-channel")]
    public string? ChatChannel { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--engagements")]
    public string[]? Engagements { get; set; }

    [CliOption("--incident-template-dedupe-string")]
    public string? IncidentTemplateDedupeString { get; set; }

    [CliOption("--incident-template-impact")]
    public int? IncidentTemplateImpact { get; set; }

    [CliOption("--incident-template-notification-targets")]
    public string[]? IncidentTemplateNotificationTargets { get; set; }

    [CliOption("--incident-template-summary")]
    public string? IncidentTemplateSummary { get; set; }

    [CliOption("--incident-template-tags")]
    public IEnumerable<KeyValue>? IncidentTemplateTags { get; set; }

    [CliOption("--incident-template-title")]
    public string? IncidentTemplateTitle { get; set; }

    [CliOption("--integrations")]
    public string[]? Integrations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}