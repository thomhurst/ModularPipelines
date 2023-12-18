using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "connection", "create", "postgres")]
public class AzWebappConnectionCreatePostgresServiceconnectorPasswordless
{
    public AzWebappConnectionCreatePostgresServiceconnectorPasswordless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzWebappConnectionCreatePostgresServiceconnectorPasswordlessExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreatePostgresServiceconnectorPasswordlessExtensionOptions(), token);
    }
}