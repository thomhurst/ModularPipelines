using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("databox")]
public class AzDataboxJob
{
    public AzDataboxJob(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Cancel(AzDataboxJobCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzDataboxJobCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDataboxJobDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxJobDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDataboxJobListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxJobListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListCredential(AzDataboxJobListCredentialOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxJobListCredentialOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> MarkDevicesShipped(AzDataboxJobMarkDevicesShippedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDataboxJobShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxJobShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDataboxJobUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataboxJobUpdateOptions(), cancellationToken: token);
    }
}