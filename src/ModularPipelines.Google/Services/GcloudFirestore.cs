using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudFirestore
{
    public GcloudFirestore(
        GcloudFirestoreDatabases databases,
        GcloudFirestoreFields fields,
        GcloudFirestoreIndexes indexes,
        GcloudFirestoreLocations locations,
        GcloudFirestoreOperations operations,
        ICommand internalCommand
    )
    {
        Databases = databases;
        Fields = fields;
        Indexes = indexes;
        Locations = locations;
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudFirestoreDatabases Databases { get; }

    public GcloudFirestoreFields Fields { get; }

    public GcloudFirestoreIndexes Indexes { get; }

    public GcloudFirestoreLocations Locations { get; }

    public GcloudFirestoreOperations Operations { get; }

    public async Task<CommandResult> Export(GcloudFirestoreExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(GcloudFirestoreImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}