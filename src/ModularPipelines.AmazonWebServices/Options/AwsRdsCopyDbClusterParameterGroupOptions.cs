using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "copy-db-cluster-parameter-group")]
public record AwsRdsCopyDbClusterParameterGroupOptions(
[property: CommandSwitch("--source-db-cluster-parameter-group-identifier")] string SourceDbClusterParameterGroupIdentifier,
[property: CommandSwitch("--target-db-cluster-parameter-group-identifier")] string TargetDbClusterParameterGroupIdentifier,
[property: CommandSwitch("--target-db-cluster-parameter-group-description")] string TargetDbClusterParameterGroupDescription
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}