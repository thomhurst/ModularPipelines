using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudWorkflows
{
    public GcloudWorkflows(
        GcloudWorkflowsExecutions executions,
        ICommand internalCommand
    )
    {
        Executions = executions;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudWorkflowsExecutions Executions { get; }

    public async Task<CommandResult> Delete(GcloudWorkflowsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deploy(GcloudWorkflowsDeployOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudWorkflowsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Execute(GcloudWorkflowsExecuteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudWorkflowsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudWorkflowsListOptions(), token);
    }

    public async Task<CommandResult> Run(GcloudWorkflowsRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}