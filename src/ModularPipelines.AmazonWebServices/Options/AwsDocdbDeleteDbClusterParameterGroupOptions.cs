using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "delete-db-cluster-parameter-group")]
public record AwsDocdbDeleteDbClusterParameterGroupOptions(
[property: CliOption("--db-cluster-parameter-group-name")] string DbClusterParameterGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}