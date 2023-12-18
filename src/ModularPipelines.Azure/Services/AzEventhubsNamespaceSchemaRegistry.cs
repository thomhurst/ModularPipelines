using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace")]
public class AzEventhubsNamespaceSchemaRegistry
{
    public AzEventhubsNamespaceSchemaRegistry(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzEventhubsNamespaceSchemaRegistryCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventhubsNamespaceSchemaRegistryDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzEventhubsNamespaceSchemaRegistryListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventhubsNamespaceSchemaRegistryShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceSchemaRegistryShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventhubsNamespaceSchemaRegistryUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceSchemaRegistryUpdateOptions(), token);
    }
}