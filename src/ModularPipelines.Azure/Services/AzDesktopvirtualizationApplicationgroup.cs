using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("desktopvirtualization")]
public class AzDesktopvirtualizationApplicationgroup
{
    public AzDesktopvirtualizationApplicationgroup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDesktopvirtualizationApplicationgroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDesktopvirtualizationApplicationgroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationApplicationgroupDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDesktopvirtualizationApplicationgroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationApplicationgroupListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDesktopvirtualizationApplicationgroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationApplicationgroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDesktopvirtualizationApplicationgroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationApplicationgroupUpdateOptions(), token);
    }
}