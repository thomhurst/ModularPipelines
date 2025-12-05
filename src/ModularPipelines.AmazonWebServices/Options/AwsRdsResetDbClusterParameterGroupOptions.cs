using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "reset-db-cluster-parameter-group")]
public record AwsRdsResetDbClusterParameterGroupOptions(
[property: CliOption("--db-cluster-parameter-group-name")] string DbClusterParameterGroupName
) : AwsOptions
{
    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}