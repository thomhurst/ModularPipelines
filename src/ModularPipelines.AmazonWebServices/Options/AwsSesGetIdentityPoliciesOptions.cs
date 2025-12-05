using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "get-identity-policies")]
public record AwsSesGetIdentityPoliciesOptions(
[property: CliOption("--identity")] string Identity,
[property: CliOption("--policy-names")] string[] PolicyNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}