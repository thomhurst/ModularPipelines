using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "send-email")]
public record AwsSesv2SendEmailOptions(
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--from-email-address")]
    public string? FromEmailAddress { get; set; }

    [CommandSwitch("--from-email-address-identity-arn")]
    public string? FromEmailAddressIdentityArn { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CommandSwitch("--feedback-forwarding-email-address")]
    public string? FeedbackForwardingEmailAddress { get; set; }

    [CommandSwitch("--feedback-forwarding-email-address-identity-arn")]
    public string? FeedbackForwardingEmailAddressIdentityArn { get; set; }

    [CommandSwitch("--email-tags")]
    public string[]? EmailTags { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--list-management-options")]
    public string? ListManagementOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}