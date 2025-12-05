using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "put-email-identity-feedback-attributes")]
public record AwsPinpointEmailPutEmailIdentityFeedbackAttributesOptions(
[property: CliOption("--email-identity")] string EmailIdentity
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}