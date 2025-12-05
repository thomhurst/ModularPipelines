using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("honeycode", "list-table-rows")]
public record AwsHoneycodeListTableRowsOptions(
[property: CliOption("--workbook-id")] string WorkbookId,
[property: CliOption("--table-id")] string TableId
) : AwsOptions
{
    [CliOption("--row-ids")]
    public string[]? RowIds { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}