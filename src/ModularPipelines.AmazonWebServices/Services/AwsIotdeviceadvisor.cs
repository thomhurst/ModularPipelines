using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsIotdeviceadvisor
{
    public AwsIotdeviceadvisor(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateSuiteDefinition(AwsIotdeviceadvisorCreateSuiteDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSuiteDefinition(AwsIotdeviceadvisorDeleteSuiteDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEndpoint(AwsIotdeviceadvisorGetEndpointOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotdeviceadvisorGetEndpointOptions(), token);
    }

    public async Task<CommandResult> GetSuiteDefinition(AwsIotdeviceadvisorGetSuiteDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSuiteRun(AwsIotdeviceadvisorGetSuiteRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSuiteRunReport(AwsIotdeviceadvisorGetSuiteRunReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSuiteDefinitions(AwsIotdeviceadvisorListSuiteDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotdeviceadvisorListSuiteDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListSuiteRuns(AwsIotdeviceadvisorListSuiteRunsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotdeviceadvisorListSuiteRunsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsIotdeviceadvisorListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSuiteRun(AwsIotdeviceadvisorStartSuiteRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopSuiteRun(AwsIotdeviceadvisorStopSuiteRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsIotdeviceadvisorTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsIotdeviceadvisorUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSuiteDefinition(AwsIotdeviceadvisorUpdateSuiteDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}