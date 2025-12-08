using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("healthcare")]
public class GcloudHealthcareHl7v2Stores
{
    public GcloudHealthcareHl7v2Stores(
        GcloudHealthcareHl7v2StoresExport export,
        GcloudHealthcareHl7v2StoresImport import,
        ICommand internalCommand
    )
    {
        Export = export;
        Import = import;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudHealthcareHl7v2StoresExport Export { get; }

    public GcloudHealthcareHl7v2StoresImport Import { get; }

    public async Task<CommandResult> AddIamPolicyBinding(GcloudHealthcareHl7v2StoresAddIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudHealthcareHl7v2StoresCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudHealthcareHl7v2StoresDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudHealthcareHl7v2StoresDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudHealthcareHl7v2StoresGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudHealthcareHl7v2StoresListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Metrics(GcloudHealthcareHl7v2StoresMetricsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveIamPolicyBinding(GcloudHealthcareHl7v2StoresRemoveIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudHealthcareHl7v2StoresSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudHealthcareHl7v2StoresUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}