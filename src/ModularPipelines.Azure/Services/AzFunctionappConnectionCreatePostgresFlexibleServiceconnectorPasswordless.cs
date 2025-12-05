using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "connection", "create", "postgres-flexible")]
public class AzFunctionappConnectionCreatePostgresFlexibleServiceconnectorPasswordless
{
    public AzFunctionappConnectionCreatePostgresFlexibleServiceconnectorPasswordless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzFunctionappConnectionCreatePostgresFlexibleServiceconnectorPasswordlessExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreatePostgresFlexibleServiceconnectorPasswordlessExtensionOptions(), token);
    }
}