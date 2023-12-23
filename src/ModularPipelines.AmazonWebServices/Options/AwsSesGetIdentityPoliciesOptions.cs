using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "get-identity-policies")]
public record AwsSesGetIdentityPoliciesOptions(
[property: CommandSwitch("--identity")] string Identity,
[property: CommandSwitch("--policy-names")] string[] PolicyNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}