using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "put-email-identity-mail-from-attributes")]
public record AwsSesv2PutEmailIdentityMailFromAttributesOptions(
[property: CommandSwitch("--email-identity")] string EmailIdentity
) : AwsOptions
{
    [CommandSwitch("--mail-from-domain")]
    public string? MailFromDomain { get; set; }

    [CommandSwitch("--behavior-on-mx-failure")]
    public string? BehaviorOnMxFailure { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}