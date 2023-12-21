using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection", "create", "postgres-flexible")]
public class AzConnectionCreatePostgresFlexibleServiceconnectorPasswordless
{
    public AzConnectionCreatePostgresFlexibleServiceconnectorPasswordless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzConnectionCreatePostgresFlexibleServiceconnectorPasswordlessExtensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}