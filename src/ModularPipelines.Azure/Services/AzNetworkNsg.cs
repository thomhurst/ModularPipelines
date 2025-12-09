using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network")]
public class AzNetworkNsg
{
    public AzNetworkNsg(
        AzNetworkNsgRule rule,
        ICommand internalCommand
    )
    {
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkNsgRule Rule { get; }

    public async Task<CommandResult> Create(AzNetworkNsgCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkNsgDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNsgDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkNsgListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNsgListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkNsgShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNsgShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkNsgUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNsgUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkNsgWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkNsgWaitOptions(), token);
    }
}