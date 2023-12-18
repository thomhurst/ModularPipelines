using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maintenance")]
public class AzMaintenanceApplyupdate
{
    public AzMaintenanceApplyupdate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMaintenanceApplyupdateCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOrUpdate(AzMaintenanceApplyupdateCreateOrUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceApplyupdateCreateOrUpdateOptions(), token);
    }

    public async Task<CommandResult> CreateOrUpdateParent(AzMaintenanceApplyupdateCreateOrUpdateParentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceApplyupdateCreateOrUpdateParentOptions(), token);
    }

    public async Task<CommandResult> List(AzMaintenanceApplyupdateListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceApplyupdateListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMaintenanceApplyupdateShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceApplyupdateShowOptions(), token);
    }

    public async Task<CommandResult> ShowParent(AzMaintenanceApplyupdateShowParentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceApplyupdateShowParentOptions(), token);
    }

    public async Task<CommandResult> Update(AzMaintenanceApplyupdateUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMaintenanceApplyupdateUpdateOptions(), token);
    }
}