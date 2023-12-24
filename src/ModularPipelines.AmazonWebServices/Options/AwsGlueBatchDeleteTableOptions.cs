using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "batch-delete-table")]
public record AwsGlueBatchDeleteTableOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--tables-to-delete")] string[] TablesToDelete
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--transaction-id")]
    public string? TransactionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}