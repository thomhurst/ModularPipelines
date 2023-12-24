using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "delete-db-cluster-endpoint")]
public record AwsNeptuneDeleteDbClusterEndpointOptions(
[property: CommandSwitch("--db-cluster-endpoint-identifier")] string DbClusterEndpointIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}