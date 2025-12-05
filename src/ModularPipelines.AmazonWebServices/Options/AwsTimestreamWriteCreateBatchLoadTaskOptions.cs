using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("timestream-write", "create-batch-load-task")]
public record AwsTimestreamWriteCreateBatchLoadTaskOptions(
[property: CliOption("--data-source-configuration")] string DataSourceConfiguration,
[property: CliOption("--report-configuration")] string ReportConfiguration,
[property: CliOption("--target-database-name")] string TargetDatabaseName,
[property: CliOption("--target-table-name")] string TargetTableName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--data-model-configuration")]
    public string? DataModelConfiguration { get; set; }

    [CliOption("--record-version")]
    public long? RecordVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}