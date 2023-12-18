using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("graph-services")]
public class AzGraphServicesAccount
{
    public AzGraphServicesAccount(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzGraphServicesAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzGraphServicesAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGraphServicesAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzGraphServicesAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGraphServicesAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzGraphServicesAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGraphServicesAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzGraphServicesAccountUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGraphServicesAccountUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzGraphServicesAccountWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGraphServicesAccountWaitOptions(), token);
    }
}