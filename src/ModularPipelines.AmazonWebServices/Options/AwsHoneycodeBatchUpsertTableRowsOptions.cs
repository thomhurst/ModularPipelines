using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("honeycode", "batch-upsert-table-rows")]
public record AwsHoneycodeBatchUpsertTableRowsOptions(
[property: CommandSwitch("--workbook-id")] string WorkbookId,
[property: CommandSwitch("--table-id")] string TableId,
[property: CommandSwitch("--rows-to-upsert")] string[] RowsToUpsert
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}