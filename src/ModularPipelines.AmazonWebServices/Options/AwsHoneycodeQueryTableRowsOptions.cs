using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("honeycode", "query-table-rows")]
public record AwsHoneycodeQueryTableRowsOptions(
[property: CommandSwitch("--workbook-id")] string WorkbookId,
[property: CommandSwitch("--table-id")] string TableId,
[property: CommandSwitch("--filter-formula")] string FilterFormula
) : AwsOptions
{
    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}