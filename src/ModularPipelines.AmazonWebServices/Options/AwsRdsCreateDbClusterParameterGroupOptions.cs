using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-db-cluster-parameter-group")]
public record AwsRdsCreateDbClusterParameterGroupOptions(
[property: CommandSwitch("--db-cluster-parameter-group-name")] string DbClusterParameterGroupName,
[property: CommandSwitch("--db-parameter-group-family")] string DbParameterGroupFamily,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}