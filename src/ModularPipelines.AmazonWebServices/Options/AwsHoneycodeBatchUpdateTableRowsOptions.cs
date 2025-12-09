using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("honeycode", "batch-update-table-rows")]
public record AwsHoneycodeBatchUpdateTableRowsOptions(
[property: CliOption("--workbook-id")] string WorkbookId,
[property: CliOption("--table-id")] string TableId,
[property: CliOption("--rows-to-update")] string[] RowsToUpdate
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}