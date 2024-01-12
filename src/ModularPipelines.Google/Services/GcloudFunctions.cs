using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudFunctions
{
    public GcloudFunctions(
        GcloudFunctionsEventTypes eventTypes,
        GcloudFunctionsLogs logs,
        GcloudFunctionsRegions regions,
        GcloudFunctionsRuntimes runtimes,
        ICommand internalCommand
    )
    {
        EventTypes = eventTypes;
        Logs = logs;
        Regions = regions;
        Runtimes = runtimes;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudFunctionsEventTypes EventTypes { get; }

    public GcloudFunctionsLogs Logs { get; }

    public GcloudFunctionsRegions Regions { get; }

    public GcloudFunctionsRuntimes Runtimes { get; }

    public async Task<CommandResult> AddIamPolicyBinding(GcloudFunctionsAddIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddInvokerPolicyBinding(GcloudFunctionsAddInvokerPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Call(GcloudFunctionsCallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudFunctionsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deploy(GcloudFunctionsDeployOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudFunctionsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudFunctionsGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudFunctionsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudFunctionsListOptions(), token);
    }

    public async Task<CommandResult> RemoveIamPolicyBinding(GcloudFunctionsRemoveIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveInvokerPolicyBinding(GcloudFunctionsRemoveInvokerPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudFunctionsSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}