using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "list-clusters")]
public record AwsEmrListClustersOptions : AwsOptions
{
    [CliOption("--created-after")]
    public long? CreatedAfter { get; set; }

    [CliOption("--created-before")]
    public long? CreatedBefore { get; set; }

    [CliOption("--cluster-states")]
    public string? ClusterStates { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}