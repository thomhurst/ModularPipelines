using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("honeycode", "describe-table-data-import-job")]
public record AwsHoneycodeDescribeTableDataImportJobOptions(
[property: CliOption("--workbook-id")] string WorkbookId,
[property: CliOption("--table-id")] string TableId,
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}