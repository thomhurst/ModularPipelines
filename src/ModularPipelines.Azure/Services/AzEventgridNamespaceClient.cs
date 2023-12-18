using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "namespace")]
public class AzEventgridNamespaceClient
{
    public AzEventgridNamespaceClient(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzEventgridNamespaceClientCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventgridNamespaceClientDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceClientDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventgridNamespaceClientListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventgridNamespaceClientShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceClientShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventgridNamespaceClientUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceClientUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzEventgridNamespaceClientWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventgridNamespaceClientWaitOptions(), token);
    }
}