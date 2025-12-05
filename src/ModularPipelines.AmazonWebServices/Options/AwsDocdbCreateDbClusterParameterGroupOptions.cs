using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "create-db-cluster-parameter-group")]
public record AwsDocdbCreateDbClusterParameterGroupOptions(
[property: CliOption("--db-cluster-parameter-group-name")] string DbClusterParameterGroupName,
[property: CliOption("--db-parameter-group-family")] string DbParameterGroupFamily,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}