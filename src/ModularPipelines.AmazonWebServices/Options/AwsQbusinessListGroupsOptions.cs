using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "list-groups")]
public record AwsQbusinessListGroupsOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--updated-earlier-than")] long UpdatedEarlierThan
) : AwsOptions
{
    [CliOption("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}