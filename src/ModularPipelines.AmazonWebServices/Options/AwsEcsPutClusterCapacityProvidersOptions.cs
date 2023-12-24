using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "put-cluster-capacity-providers")]
public record AwsEcsPutClusterCapacityProvidersOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--capacity-providers")] string[] CapacityProviders,
[property: CommandSwitch("--default-capacity-provider-strategy")] string[] DefaultCapacityProviderStrategy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}