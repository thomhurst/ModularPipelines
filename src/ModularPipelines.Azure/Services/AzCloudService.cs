using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzCloudService
{
    public AzCloudService(
        AzCloudServiceRole role,
        AzCloudServiceRoleInstance roleInstance,
        AzCloudServiceUpdateDomain updateDomain,
        ICommand internalCommand
    )
    {
        Role = role;
        RoleInstance = roleInstance;
        UpdateDomain = updateDomain;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCloudServiceRole Role { get; }

    public AzCloudServiceRoleInstance RoleInstance { get; }

    public AzCloudServiceUpdateDomain UpdateDomain { get; }

    public async Task<CommandResult> Create(AzCloudServiceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzCloudServiceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> DeleteInstance(AzCloudServiceDeleteInstanceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceDeleteInstanceOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzCloudServiceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListAll(AzCloudServiceListAllOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceListAllOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> PowerOff(AzCloudServicePowerOffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServicePowerOffOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Rebuild(AzCloudServiceRebuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRebuildOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Reimage(AzCloudServiceReimageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceReimageOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restart(AzCloudServiceRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRestartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzCloudServiceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowInstanceView(AzCloudServiceShowInstanceViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceShowInstanceViewOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzCloudServiceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzCloudServiceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzCloudServiceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceWaitOptions(), cancellationToken: token);
    }
}