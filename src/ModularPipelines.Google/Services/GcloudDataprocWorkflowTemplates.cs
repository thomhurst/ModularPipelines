using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataproc")]
public class GcloudDataprocWorkflowTemplates
{
    public GcloudDataprocWorkflowTemplates(
        GcloudDataprocWorkflowTemplatesAddJob addJob,
        ICommand internalCommand
    )
    {
        AddJob = addJob;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDataprocWorkflowTemplatesAddJob AddJob { get; }

    public async Task<CommandResult> Create(GcloudDataprocWorkflowTemplatesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudDataprocWorkflowTemplatesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudDataprocWorkflowTemplatesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(GcloudDataprocWorkflowTemplatesExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudDataprocWorkflowTemplatesGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(GcloudDataprocWorkflowTemplatesImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Instantiate(GcloudDataprocWorkflowTemplatesInstantiateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InstantiateFromFile(GcloudDataprocWorkflowTemplatesInstantiateFromFileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudDataprocWorkflowTemplatesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudDataprocWorkflowTemplatesListOptions(), token);
    }

    public async Task<CommandResult> RemoveDagTimeout(GcloudDataprocWorkflowTemplatesRemoveDagTimeoutOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveJob(GcloudDataprocWorkflowTemplatesRemoveJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetClusterSelector(GcloudDataprocWorkflowTemplatesSetClusterSelectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetDagTimeout(GcloudDataprocWorkflowTemplatesSetDagTimeoutOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudDataprocWorkflowTemplatesSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetManagedCluster(GcloudDataprocWorkflowTemplatesSetManagedClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}