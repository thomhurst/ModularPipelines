using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "send-bulk-email")]
public record AwsSesv2SendBulkEmailOptions(
[property: CliOption("--default-content")] string DefaultContent,
[property: CliOption("--bulk-email-entries")] string[] BulkEmailEntries
) : AwsOptions
{
    [CliOption("--from-email-address")]
    public string? FromEmailAddress { get; set; }

    [CliOption("--from-email-address-identity-arn")]
    public string? FromEmailAddressIdentityArn { get; set; }

    [CliOption("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CliOption("--feedback-forwarding-email-address")]
    public string? FeedbackForwardingEmailAddress { get; set; }

    [CliOption("--feedback-forwarding-email-address-identity-arn")]
    public string? FeedbackForwardingEmailAddressIdentityArn { get; set; }

    [CliOption("--default-email-tags")]
    public string[]? DefaultEmailTags { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}