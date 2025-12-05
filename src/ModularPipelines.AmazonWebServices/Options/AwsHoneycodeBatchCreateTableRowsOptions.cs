using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("honeycode", "batch-create-table-rows")]
public record AwsHoneycodeBatchCreateTableRowsOptions(
[property: CliOption("--workbook-id")] string WorkbookId,
[property: CliOption("--table-id")] string TableId,
[property: CliOption("--rows-to-create")] string[] RowsToCreate
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}