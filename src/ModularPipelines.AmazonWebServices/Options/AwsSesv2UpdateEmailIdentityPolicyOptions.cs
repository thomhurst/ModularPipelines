using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "update-email-identity-policy")]
public record AwsSesv2UpdateEmailIdentityPolicyOptions(
[property: CliOption("--email-identity")] string EmailIdentity,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}