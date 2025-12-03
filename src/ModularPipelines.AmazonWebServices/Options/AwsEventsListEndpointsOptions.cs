using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "list-endpoints")]
public record AwsEventsListEndpointsOptions : AwsOptions
{
    [CliOption("--name-prefix")]
    public string? NamePrefix { get; set; }

    [CliOption("--home-region")]
    public string? HomeRegion { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}