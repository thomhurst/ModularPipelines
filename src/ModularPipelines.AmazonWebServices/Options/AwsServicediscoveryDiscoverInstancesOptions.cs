using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicediscovery", "discover-instances")]
public record AwsServicediscoveryDiscoverInstancesOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--service-name")] string ServiceName
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--query-parameters")]
    public IEnumerable<KeyValue>? QueryParameters { get; set; }

    [CommandSwitch("--optional-parameters")]
    public IEnumerable<KeyValue>? OptionalParameters { get; set; }

    [CommandSwitch("--health-status")]
    public string? HealthStatus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}