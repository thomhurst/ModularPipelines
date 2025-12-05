using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "describe-clusters")]
public record AwsEcsDescribeClustersOptions : AwsOptions
{
    [CliOption("--clusters")]
    public string[]? Clusters { get; set; }

    [CliOption("--include")]
    public string[]? Include { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}