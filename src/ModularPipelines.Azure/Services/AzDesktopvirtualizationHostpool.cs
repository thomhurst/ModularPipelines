using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("desktopvirtualization")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDesktopvirtualizationHostpoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationHostpoolDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDesktopvirtualizationHostpoolListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationHostpoolListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RetrieveRegistrationToken(AzDesktopvirtualizationHostpoolRetrieveRegistrationTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationHostpoolRetrieveRegistrationTokenOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDesktopvirtualizationHostpoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationHostpoolShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDesktopvirtualizationHostpoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDesktopvirtualizationHostpoolUpdateOptions(), cancellationToken: token);
    }
}