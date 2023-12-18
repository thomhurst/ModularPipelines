using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksMaintenanceconfiguration
{
    public AzAksMaintenanceconfiguration(
        AzAksMaintenanceconfigurationAdd add,
        AzAksMaintenanceconfigurationDelete delete,
        AzAksMaintenanceconfigurationList list,
        AzAksMaintenanceconfigurationShow show,
        AzAksMaintenanceconfigurationUpdate update,
        ICommand internalCommand
    )
    {
        AddCommands = add;
        DeleteCommands = delete;
        ListCommands = list;
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAksMaintenanceconfigurationAdd AddCommands { get; }

    public AzAksMaintenanceconfigurationDelete DeleteCommands { get; }

    public AzAksMaintenanceconfigurationList ListCommands { get; }

    public AzAksMaintenanceconfigurationShow ShowCommands { get; }

    public AzAksMaintenanceconfigurationUpdate UpdateCommands { get; }

    public async Task<CommandResult> Add(AzAksMaintenanceconfigurationAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAksMaintenanceconfigurationDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAksMaintenanceconfigurationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAksMaintenanceconfigurationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzAksMaintenanceconfigurationUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}