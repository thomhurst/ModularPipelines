using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("databoxedge")]
public class AzDataboxedgeOrder
{
    public AzDataboxedgeOrder(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDataboxedgeOrderCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDataboxedgeOrderDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeOrderDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDataboxedgeOrderListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDataboxedgeOrderShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeOrderShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDataboxedgeOrderUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeOrderUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDataboxedgeOrderWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxedgeOrderWaitOptions(), cancellationToken: token);
    }
}