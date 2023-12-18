using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connection", "create", "postgres-flexible")]
public class AzContainerappConnectionCreatePostgresFlexibleServiceconnectorPasswordless
{
    public AzContainerappConnectionCreatePostgresFlexibleServiceconnectorPasswordless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzContainerappConnectionCreatePostgresFlexibleServiceconnectorPasswordlessExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreatePostgresFlexibleServiceconnectorPasswordlessExtensionOptions(), token);
    }
}