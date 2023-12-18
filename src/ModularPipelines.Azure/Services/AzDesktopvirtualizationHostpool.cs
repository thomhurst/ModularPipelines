using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("desktopvirtualization")]
public class AzDesktopvirtualizationHostpool
{
    public AzDesktopvirtualizationHostpool(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDesktopvirtualizationHostpoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDesktopvirtualizationHostpoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationHostpoolDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDesktopvirtualizationHostpoolListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationHostpoolListOptions(), token);
    }

    public async Task<CommandResult> RetrieveRegistrationToken(AzDesktopvirtualizationHostpoolRetrieveRegistrationTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationHostpoolRetrieveRegistrationTokenOptions(), token);
    }

    public async Task<CommandResult> Show(AzDesktopvirtualizationHostpoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationHostpoolShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDesktopvirtualizationHostpoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationHostpoolUpdateOptions(), token);
    }
}

