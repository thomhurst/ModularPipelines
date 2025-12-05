using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "list-discoverers")]
public record AwsSchemasListDiscoverersOptions : AwsOptions
{
    [CliOption("--discoverer-id-prefix")]
    public string? DiscovererIdPrefix { get; set; }

    [CliOption("--source-arn-prefix")]
    public string? SourceArnPrefix { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}