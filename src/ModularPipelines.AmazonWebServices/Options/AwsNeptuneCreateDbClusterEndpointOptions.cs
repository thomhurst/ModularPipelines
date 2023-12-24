using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "create-db-cluster-endpoint")]
public record AwsNeptuneCreateDbClusterEndpointOptions(
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier,
[property: CommandSwitch("--db-cluster-endpoint-identifier")] string DbClusterEndpointIdentifier,
[property: CommandSwitch("--endpoint-type")] string EndpointType
) : AwsOptions
{
    [CommandSwitch("--static-members")]
    public string[]? StaticMembers { get; set; }

    [CommandSwitch("--excluded-members")]
    public string[]? ExcludedMembers { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}