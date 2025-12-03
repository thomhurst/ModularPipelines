using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("honeycode", "start-table-data-import-job")]
public record AwsHoneycodeStartTableDataImportJobOptions(
[property: CliOption("--workbook-id")] string WorkbookId,
[property: CliOption("--data-source")] string DataSource,
[property: CliOption("--data-format")] string DataFormat,
[property: CliOption("--destination-table-id")] string DestinationTableId,
[property: CliOption("--import-options")] string ImportOptions,
[property: CliOption("--client-request-token")] string ClientRequestToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}