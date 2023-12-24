using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("honeycode", "batch-delete-table-rows")]
public record AwsHoneycodeBatchDeleteTableRowsOptions(
[property: CommandSwitch("--workbook-id")] string WorkbookId,
[property: CommandSwitch("--table-id")] string TableId,
[property: CommandSwitch("--row-ids")] string[] RowIds
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}