using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "create-email-identity-policy")]
public record AwsSesv2CreateEmailIdentityPolicyOptions(
[property: CommandSwitch("--email-identity")] string EmailIdentity,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--policy")] string Policy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}