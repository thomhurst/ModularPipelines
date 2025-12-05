using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("maps")]
public class AzMapsAccount
{
    public AzMapsAccount(
        AzMapsAccountKeys keys,
        ICommand internalCommand
    )
    {
        Keys = keys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMapsAccountKeys Keys { get; }

    public async Task<CommandResult> Create(AzMapsAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMapsAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMapsAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMapsAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMapsAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMapsAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMapsAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMapsAccountUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}