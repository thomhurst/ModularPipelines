using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-service")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzCloudServiceRoleInstanceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rebuild(AzCloudServiceRoleInstanceRebuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceRebuildOptions(), token);
    }

    public async Task<CommandResult> Reimage(AzCloudServiceRoleInstanceReimageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceReimageOptions(), token);
    }

    public async Task<CommandResult> Restart(AzCloudServiceRoleInstanceRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzCloudServiceRoleInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceShowOptions(), token);
    }

    public async Task<CommandResult> ShowInstanceView(AzCloudServiceRoleInstanceShowInstanceViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceShowInstanceViewOptions(), token);
    }

    public async Task<CommandResult> ShowRemoteDesktopFile(AzCloudServiceRoleInstanceShowRemoteDesktopFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRoleInstanceShowRemoteDesktopFileOptions(), token);
    }
}