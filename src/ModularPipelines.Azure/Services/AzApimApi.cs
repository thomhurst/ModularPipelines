using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim")]
public class AzApimApi
{
    public AzApimApi(
        AzApimApiOperation operation,
        AzApimApiRelease release,
        AzApimApiRevision revision,
        AzApimApiSchema schema,
        AzApimApiVersionset versionset,
        ICommand internalCommand
    )
    {
        Operation = operation;
        Release = release;
        Revision = revision;
        Schema = schema;
        Versionset = versionset;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzApimApiOperation Operation { get; }

    public AzApimApiRelease Release { get; }

    public AzApimApiRevision Revision { get; }

    public AzApimApiSchema Schema { get; }

    public AzApimApiVersionset Versionset { get; }

    public async Task<CommandResult> Create(AzApimApiCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzApimApiDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(AzApimApiImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzApimApiListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzApimApiShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzApimApiUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzApimApiWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}