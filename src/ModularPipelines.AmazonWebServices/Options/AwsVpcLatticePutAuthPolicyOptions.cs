using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "put-auth-policy")]
public record AwsVpcLatticePutAuthPolicyOptions(
[property: CliOption("--policy")] string Policy,
[property: CliOption("--resource-identifier")] string ResourceIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}