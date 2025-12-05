using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "delete-email-identity-policy")]
public record AwsSesv2DeleteEmailIdentityPolicyOptions(
[property: CliOption("--email-identity")] string EmailIdentity,
[property: CliOption("--policy-name")] string PolicyName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}