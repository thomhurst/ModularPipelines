using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "put-identity-policy")]
public record AwsSesPutIdentityPolicyOptions(
[property: CommandSwitch("--identity")] string Identity,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--policy")] string Policy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}