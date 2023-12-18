using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "config")]
public class AzFunctionappConfigContainer
{
    public AzFunctionappConfigContainer(
        AzFunctionappConfigContainerSet set,
        ICommand internalCommand
    )
    {
        SetCommands = set;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzFunctionappConfigContainerSet SetCommands { get; }

    public async Task<CommandResult> Delete(AzFunctionappConfigContainerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConfigContainerDeleteOptions(), token);
    }

    public async Task<CommandResult> Set(AzFunctionappConfigContainerSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConfigContainerSetOptions(), token);
    }

    public async Task<CommandResult> Show(AzFunctionappConfigContainerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConfigContainerShowOptions(), token);
    }
}