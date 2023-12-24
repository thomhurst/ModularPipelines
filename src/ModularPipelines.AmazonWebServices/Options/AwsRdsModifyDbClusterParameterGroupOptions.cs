using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-cluster-parameter-group")]
public record AwsRdsModifyDbClusterParameterGroupOptions(
[property: CommandSwitch("--db-cluster-parameter-group-name")] string DbClusterParameterGroupName,
[property: CommandSwitch("--parameters")] string[] Parameters
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}