using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;

namespace ModularPipelines.DotNet.Services;

[ExcludeFromCodeCoverage]
public class DotNetWorkload
{
    public DotNetWorkload(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public virtual async Task<CommandResult> Install(DotNetWorkloadInstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> List(DotNetWorkloadListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetWorkloadListOptions(), null, token);
    }

    public virtual async Task<CommandResult> Update(DotNetWorkloadUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetWorkloadUpdateOptions(), null, token);
    }

    public virtual async Task<CommandResult> Restore(DotNetWorkloadRestoreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetWorkloadRestoreOptions(), null, token);
    }

    public virtual async Task<CommandResult> Repair(DotNetWorkloadRepairOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetWorkloadRepairOptions(), null, token);
    }

    public virtual async Task<CommandResult> Uninstall(DotNetWorkloadUninstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Search(DotNetWorkloadSearchOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetWorkloadSearchOptions(), null, token);
    }
}
