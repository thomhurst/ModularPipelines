using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "list-budgets-for-resource")]
public record AwsServicecatalogListBudgetsForResourceOptions(
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}