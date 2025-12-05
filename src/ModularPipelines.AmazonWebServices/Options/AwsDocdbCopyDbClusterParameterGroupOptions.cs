using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "copy-db-cluster-parameter-group")]
public record AwsDocdbCopyDbClusterParameterGroupOptions(
[property: CliOption("--source-db-cluster-parameter-group-identifier")] string SourceDbClusterParameterGroupIdentifier,
[property: CliOption("--target-db-cluster-parameter-group-identifier")] string TargetDbClusterParameterGroupIdentifier,
[property: CliOption("--target-db-cluster-parameter-group-description")] string TargetDbClusterParameterGroupDescription
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}