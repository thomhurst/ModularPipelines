using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "auth")]
public class AzContainerappAuthTwitter
{
    public AzContainerappAuthTwitter(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzContainerappAuthTwitterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappAuthTwitterShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzContainerappAuthTwitterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappAuthTwitterUpdateOptions(), token);
    }
}