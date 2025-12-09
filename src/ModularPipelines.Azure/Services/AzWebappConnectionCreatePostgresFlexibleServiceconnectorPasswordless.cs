using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "connection", "create", "postgres-flexible")]
public class AzWebappConnectionCreatePostgresFlexibleServiceconnectorPasswordless
{
    public AzWebappConnectionCreatePostgresFlexibleServiceconnectorPasswordless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzWebappConnectionCreatePostgresFlexibleServiceconnectorPasswordlessExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreatePostgresFlexibleServiceconnectorPasswordlessExtensionOptions(), token);
    }
}