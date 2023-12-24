using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "get-data-cells-filter")]
public record AwsLakeformationGetDataCellsFilterOptions(
[property: CommandSwitch("--table-catalog-id")] string TableCatalogId,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}