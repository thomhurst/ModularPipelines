using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "list-clusters-v2")]
public record AwsKafkaListClustersV2Options : AwsOptions
{
    [CliOption("--cluster-name-filter")]
    public string? ClusterNameFilter { get; set; }

    [CliOption("--cluster-type-filter")]
    public string? ClusterTypeFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}