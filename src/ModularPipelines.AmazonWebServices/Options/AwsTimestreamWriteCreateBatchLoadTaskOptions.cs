using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("timestream-write", "create-batch-load-task")]
public record AwsTimestreamWriteCreateBatchLoadTaskOptions(
[property: CommandSwitch("--data-source-configuration")] string DataSourceConfiguration,
[property: CommandSwitch("--report-configuration")] string ReportConfiguration,
[property: CommandSwitch("--target-database-name")] string TargetDatabaseName,
[property: CommandSwitch("--target-table-name")] string TargetTableName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--data-model-configuration")]
    public string? DataModelConfiguration { get; set; }

    [CommandSwitch("--record-version")]
    public long? RecordVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}