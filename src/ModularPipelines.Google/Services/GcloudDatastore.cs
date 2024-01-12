using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDatastore
{
    public GcloudDatastore(
        GcloudDatastoreIndexes indexes,
        GcloudDatastoreOperations operations,
        ICommand internalCommand
    )
    {
        Indexes = indexes;
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDatastoreIndexes Indexes { get; }

    public GcloudDatastoreOperations Operations { get; }

    public async Task<CommandResult> Export(GcloudDatastoreExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(GcloudDatastoreImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}