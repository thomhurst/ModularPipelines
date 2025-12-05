using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-global-cluster")]
public record AwsRdsCreateGlobalClusterOptions : AwsOptions
{
    [CliOption("--global-cluster-identifier")]
    public string? GlobalClusterIdentifier { get; set; }

    [CliOption("--source-db-cluster-identifier")]
    public string? SourceDbClusterIdentifier { get; set; }

    [CliOption("--engine")]
    public string? Engine { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}