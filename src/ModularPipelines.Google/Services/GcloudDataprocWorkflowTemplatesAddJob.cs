using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates")]
public class GcloudDataprocWorkflowTemplatesAddJob
{
    public GcloudDataprocWorkflowTemplatesAddJob(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Hadoop(GcloudDataprocWorkflowTemplatesAddJobHadoopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Hive(GcloudDataprocWorkflowTemplatesAddJobHiveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Pig(GcloudDataprocWorkflowTemplatesAddJobPigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Presto(GcloudDataprocWorkflowTemplatesAddJobPrestoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Pyspark(GcloudDataprocWorkflowTemplatesAddJobPysparkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Spark(GcloudDataprocWorkflowTemplatesAddJobSparkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SparkR(GcloudDataprocWorkflowTemplatesAddJobSparkROptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SparkSql(GcloudDataprocWorkflowTemplatesAddJobSparkSqlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Trino(GcloudDataprocWorkflowTemplatesAddJobTrinoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}