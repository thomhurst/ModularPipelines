using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicediscovery", "get-instances-health-status")]
public record AwsServicediscoveryGetInstancesHealthStatusOptions(
[property: CliOption("--service-id")] string ServiceId
) : AwsOptions
{
    [CliOption("--instances")]
    public string[]? Instances { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}