using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "describe-identity-provider-config")]
public record AwsEksDescribeIdentityProviderConfigOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--identity-provider-config")] string IdentityProviderConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}