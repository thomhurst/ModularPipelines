using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "set-identity-feedback-forwarding-enabled")]
public record AwsSesSetIdentityFeedbackForwardingEnabledOptions(
[property: CommandSwitch("--identity")] string Identity
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}