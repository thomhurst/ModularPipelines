using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "put-identity-policy")]
public record AwsSesPutIdentityPolicyOptions(
[property: CliOption("--identity")] string Identity,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}