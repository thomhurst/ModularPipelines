using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-cost-categories")]
public record AwsCeGetCostCategoriesOptions(
[property: CommandSwitch("--time-period")] string TimePeriod
) : AwsOptions
{
    [CommandSwitch("--search-string")]
    public string? SearchString { get; set; }

    [CommandSwitch("--cost-category-name")]
    public string? CostCategoryName { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--sort-by")]
    public string[]? SortBy { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}