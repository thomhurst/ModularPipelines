using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicediscovery", "discover-instances")]
public record AwsServicediscoveryDiscoverInstancesOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--query-parameters")]
    public IEnumerable<KeyValue>? QueryParameters { get; set; }

    [CliOption("--optional-parameters")]
    public IEnumerable<KeyValue>? OptionalParameters { get; set; }

    [CliOption("--health-status")]
    public string? HealthStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}