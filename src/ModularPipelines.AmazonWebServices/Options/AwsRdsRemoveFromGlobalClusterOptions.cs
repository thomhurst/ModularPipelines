using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "remove-from-global-cluster")]
public record AwsRdsRemoveFromGlobalClusterOptions : AwsOptions
{
    [CliOption("--global-cluster-identifier")]
    public string? GlobalClusterIdentifier { get; set; }

    [CliOption("--db-cluster-identifier")]
    public string? DbClusterIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}