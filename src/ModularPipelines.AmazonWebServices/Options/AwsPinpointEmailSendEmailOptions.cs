using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "send-email")]
public record AwsPinpointEmailSendEmailOptions(
[property: CliOption("--destination")] string Destination,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--from-email-address")]
    public string? FromEmailAddress { get; set; }

    [CliOption("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CliOption("--feedback-forwarding-email-address")]
    public string? FeedbackForwardingEmailAddress { get; set; }

    [CliOption("--email-tags")]
    public string[]? EmailTags { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}