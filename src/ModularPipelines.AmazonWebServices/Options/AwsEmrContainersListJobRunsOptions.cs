using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-containers", "list-job-runs")]
public record AwsEmrContainersListJobRunsOptions(
[property: CliOption("--virtual-cluster-id")] string VirtualClusterId
) : AwsOptions
{
    [CliOption("--created-before")]
    public long? CreatedBefore { get; set; }

    [CliOption("--created-after")]
    public long? CreatedAfter { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--states")]
    public string[]? States { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}