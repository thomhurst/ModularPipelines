using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "delete-db-cluster-parameter-group")]
public record AwsRdsDeleteDbClusterParameterGroupOptions(
[property: CommandSwitch("--db-cluster-parameter-group-name")] string DbClusterParameterGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}