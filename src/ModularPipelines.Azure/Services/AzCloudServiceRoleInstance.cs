using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloud-service")]
public class AzCloudServiceRoleInstance
{
    public AzCloudServiceRoleInstance(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzCloudServiceRoleInstanceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzCloudServiceRoleInstanceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Rebuild(AzCloudServiceRoleInstanceRebuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceRebuildOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Reimage(AzCloudServiceRoleInstanceReimageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceReimageOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restart(AzCloudServiceRoleInstanceRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceRestartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzCloudServiceRoleInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowInstanceView(AzCloudServiceRoleInstanceShowInstanceViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceShowInstanceViewOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowRemoteDesktopFile(AzCloudServiceRoleInstanceShowRemoteDesktopFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceShowRemoteDesktopFileOptions(), cancellationToken: token);
    }
}