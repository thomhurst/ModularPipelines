using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice")]
public class AzAppservicePlan
{
    public AzAppservicePlan(
        AzAppservicePlanCreate create,
        AzAppservicePlanList list,
        AzAppservicePlanShow show,
        AzAppservicePlanUpdate update,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        ListCommands = list;
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAppservicePlanCreate CreateCommands { get; }

    public AzAppservicePlanList ListCommands { get; }

    public AzAppservicePlanShow ShowCommands { get; }

    public AzAppservicePlanUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzAppservicePlanCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAppservicePlanDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppservicePlanDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAppservicePlanListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppservicePlanListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAppservicePlanShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppservicePlanShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAppservicePlanUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAppservicePlanUpdateOptions(), token);
    }
}