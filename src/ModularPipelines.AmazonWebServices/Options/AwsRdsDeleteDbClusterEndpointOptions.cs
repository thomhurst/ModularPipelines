using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "delete-db-cluster-endpoint")]
public record AwsRdsDeleteDbClusterEndpointOptions(
[property: CommandSwitch("--db-cluster-endpoint-identifier")] string DbClusterEndpointIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}