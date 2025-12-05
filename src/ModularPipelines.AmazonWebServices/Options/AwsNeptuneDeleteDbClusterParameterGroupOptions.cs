using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "delete-db-cluster-parameter-group")]
public record AwsNeptuneDeleteDbClusterParameterGroupOptions(
[property: CliOption("--db-cluster-parameter-group-name")] string DbClusterParameterGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}