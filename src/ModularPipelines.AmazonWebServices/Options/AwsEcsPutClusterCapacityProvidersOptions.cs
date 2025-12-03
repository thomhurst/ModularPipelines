using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "put-cluster-capacity-providers")]
public record AwsEcsPutClusterCapacityProvidersOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--capacity-providers")] string[] CapacityProviders,
[property: CliOption("--default-capacity-provider-strategy")] string[] DefaultCapacityProviderStrategy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}