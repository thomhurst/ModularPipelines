using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "update-data-cells-filter")]
public record AwsLakeformationUpdateDataCellsFilterOptions(
[property: CliOption("--table-data")] string TableData
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}