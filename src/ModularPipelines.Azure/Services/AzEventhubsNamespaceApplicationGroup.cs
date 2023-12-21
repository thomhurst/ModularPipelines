using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace")]
public class AzEventhubsNamespaceApplicationGroup
{
    public AzEventhubsNamespaceApplicationGroup(
        AzEventhubsNamespaceApplicationGroupPolicy policy,
        ICommand internalCommand
    )
    {
        Policy = policy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventhubsNamespaceApplicationGroupPolicy Policy { get; }

    public async Task<CommandResult> Create(AzEventhubsNamespaceApplicationGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventhubsNamespaceApplicationGroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceApplicationGroupDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventhubsNamespaceApplicationGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventhubsNamespaceApplicationGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceApplicationGroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventhubsNamespaceApplicationGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceApplicationGroupUpdateOptions(), token);
    }
}