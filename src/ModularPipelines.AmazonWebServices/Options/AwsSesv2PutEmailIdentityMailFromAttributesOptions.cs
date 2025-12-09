using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "put-email-identity-mail-from-attributes")]
public record AwsSesv2PutEmailIdentityMailFromAttributesOptions(
[property: CliOption("--email-identity")] string EmailIdentity
) : AwsOptions
{
    [CliOption("--mail-from-domain")]
    public string? MailFromDomain { get; set; }

    [CliOption("--behavior-on-mx-failure")]
    public string? BehaviorOnMxFailure { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}