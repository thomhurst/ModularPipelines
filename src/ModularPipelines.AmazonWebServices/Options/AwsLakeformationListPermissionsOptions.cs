using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "list-permissions")]
public record AwsLakeformationListPermissionsOptions : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--principal")]
    public string? Principal { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--include-related")]
    public string? IncludeRelated { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}