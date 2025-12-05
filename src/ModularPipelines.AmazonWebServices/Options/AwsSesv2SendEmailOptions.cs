using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "send-email")]
public record AwsSesv2SendEmailOptions(
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--from-email-address")]
    public string? FromEmailAddress { get; set; }

    [CliOption("--from-email-address-identity-arn")]
    public string? FromEmailAddressIdentityArn { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CliOption("--feedback-forwarding-email-address")]
    public string? FeedbackForwardingEmailAddress { get; set; }

    [CliOption("--feedback-forwarding-email-address-identity-arn")]
    public string? FeedbackForwardingEmailAddressIdentityArn { get; set; }

    [CliOption("--email-tags")]
    public string[]? EmailTags { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--list-management-options")]
    public string? ListManagementOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}