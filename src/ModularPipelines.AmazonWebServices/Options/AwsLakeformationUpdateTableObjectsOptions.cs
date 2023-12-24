using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "update-table-objects")]
public record AwsLakeformationUpdateTableObjectsOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--write-operations")] string[] WriteOperations
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--transaction-id")]
    public string? TransactionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}