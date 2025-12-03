using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "put-email-identity-mail-from-attributes")]
public record AwsPinpointEmailPutEmailIdentityMailFromAttributesOptions(
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