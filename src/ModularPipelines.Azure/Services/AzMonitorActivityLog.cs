using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor")]
public class AzMonitorActivityLog
{
    public AzMonitorActivityLog(
        AzMonitorActivityLogAlert alert,
        ICommand internalCommand
    )
    {
        Alert = alert;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorActivityLogAlert Alert { get; }

    public async Task<CommandResult> List(AzMonitorActivityLogListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorActivityLogListOptions(), token);
    }

    public async Task<CommandResult> ListCategories(AzMonitorActivityLogListCategoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorActivityLogListCategoriesOptions(), token);
    }
}