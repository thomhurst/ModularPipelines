using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "list-groups-older-than-ordering-id")]
public record AwsKendraListGroupsOlderThanOrderingIdOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--ordering-id")] long OrderingId
) : AwsOptions
{
    [CliOption("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}