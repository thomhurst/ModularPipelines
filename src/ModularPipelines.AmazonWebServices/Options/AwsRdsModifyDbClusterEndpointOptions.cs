using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-cluster-endpoint")]
public record AwsRdsModifyDbClusterEndpointOptions(
[property: CommandSwitch("--db-cluster-endpoint-identifier")] string DbClusterEndpointIdentifier
) : AwsOptions
{
    [CommandSwitch("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CommandSwitch("--static-members")]
    public string[]? StaticMembers { get; set; }

    [CommandSwitch("--excluded-members")]
    public string[]? ExcludedMembers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}