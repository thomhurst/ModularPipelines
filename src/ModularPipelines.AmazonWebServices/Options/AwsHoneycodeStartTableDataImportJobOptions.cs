using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("honeycode", "start-table-data-import-job")]
public record AwsHoneycodeStartTableDataImportJobOptions(
[property: CommandSwitch("--workbook-id")] string WorkbookId,
[property: CommandSwitch("--data-source")] string DataSource,
[property: CommandSwitch("--data-format")] string DataFormat,
[property: CommandSwitch("--destination-table-id")] string DestinationTableId,
[property: CommandSwitch("--import-options")] string ImportOptions,
[property: CommandSwitch("--client-request-token")] string ClientRequestToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}