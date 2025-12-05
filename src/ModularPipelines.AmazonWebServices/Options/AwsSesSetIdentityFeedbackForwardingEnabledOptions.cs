using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "set-identity-feedback-forwarding-enabled")]
public record AwsSesSetIdentityFeedbackForwardingEnabledOptions(
[property: CliOption("--identity")] string Identity
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}