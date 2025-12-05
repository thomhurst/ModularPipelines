using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-global-cluster")]
public record AwsRdsModifyGlobalClusterOptions : AwsOptions
{
    [CliOption("--global-cluster-identifier")]
    public string? GlobalClusterIdentifier { get; set; }

    [CliOption("--new-global-cluster-identifier")]
    public string? NewGlobalClusterIdentifier { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}