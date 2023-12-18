using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCloudServiceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstance(AzCloudServiceDeleteInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCloudServiceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAll(AzCloudServiceListAllOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceListAllOptions(), token);
    }

    public async Task<CommandResult> PowerOff(AzCloudServicePowerOffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServicePowerOffOptions(), token);
    }

    public async Task<CommandResult> Rebuild(AzCloudServiceRebuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRebuildOptions(), token);
    }

    public async Task<CommandResult> Reimage(AzCloudServiceReimageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceReimageOptions(), token);
    }

    public async Task<CommandResult> Restart(AzCloudServiceRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzCloudServiceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceShowOptions(), token);
    }

    public async Task<CommandResult> ShowInstanceView(AzCloudServiceShowInstanceViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceShowInstanceViewOptions(), token);
    }

    public async Task<CommandResult> Start(AzCloudServiceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceStartOptions(), token);
    }

    public async Task<CommandResult> Update(AzCloudServiceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzCloudServiceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceWaitOptions(), token);
    }
}