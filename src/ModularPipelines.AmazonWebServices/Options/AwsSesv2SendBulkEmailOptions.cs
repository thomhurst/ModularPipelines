using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "send-bulk-email")]
public record AwsSesv2SendBulkEmailOptions(
[property: CommandSwitch("--default-content")] string DefaultContent,
[property: CommandSwitch("--bulk-email-entries")] string[] BulkEmailEntries
) : AwsOptions
{
    [CommandSwitch("--from-email-address")]
    public string? FromEmailAddress { get; set; }

    [CommandSwitch("--from-email-address-identity-arn")]
    public string? FromEmailAddressIdentityArn { get; set; }

    [CommandSwitch("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CommandSwitch("--feedback-forwarding-email-address")]
    public string? FeedbackForwardingEmailAddress { get; set; }

    [CommandSwitch("--feedback-forwarding-email-address-identity-arn")]
    public string? FeedbackForwardingEmailAddressIdentityArn { get; set; }

    [CommandSwitch("--default-email-tags")]
    public string[]? DefaultEmailTags { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}