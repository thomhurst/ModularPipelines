using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-contacts", "start-engagement")]
public record AwsSsmContactsStartEngagementOptions(
[property: CommandSwitch("--contact-id")] string ContactId,
[property: CommandSwitch("--sender")] string Sender,
[property: CommandSwitch("--subject")] string Subject,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--public-subject")]
    public string? PublicSubject { get; set; }

    [CommandSwitch("--public-content")]
    public string? PublicContent { get; set; }

    [CommandSwitch("--incident-id")]
    public string? IncidentId { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}