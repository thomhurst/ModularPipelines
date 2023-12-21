using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config")]
public class AzWebappConfigContainer
{
    public AzWebappConfigContainer(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzWebappConfigContainerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConfigContainerDeleteOptions(), token);
    }

    public async Task<CommandResult> Set(AzWebappConfigContainerSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConfigContainerSetOptions(), token);
    }

    public async Task<CommandResult> Show(AzWebappConfigContainerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConfigContainerShowOptions(), token);
    }
}