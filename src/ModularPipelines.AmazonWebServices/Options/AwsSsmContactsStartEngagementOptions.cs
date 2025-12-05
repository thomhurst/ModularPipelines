using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-contacts", "start-engagement")]
public record AwsSsmContactsStartEngagementOptions(
[property: CliOption("--contact-id")] string ContactId,
[property: CliOption("--sender")] string Sender,
[property: CliOption("--subject")] string Subject,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--public-subject")]
    public string? PublicSubject { get; set; }

    [CliOption("--public-content")]
    public string? PublicContent { get; set; }

    [CliOption("--incident-id")]
    public string? IncidentId { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}