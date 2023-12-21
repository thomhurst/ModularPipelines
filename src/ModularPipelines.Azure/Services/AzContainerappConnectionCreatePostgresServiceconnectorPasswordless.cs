using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connection", "create", "postgres")]
public class AzContainerappConnectionCreatePostgresServiceconnectorPasswordless
{
    public AzContainerappConnectionCreatePostgresServiceconnectorPasswordless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzContainerappConnectionCreatePostgresServiceconnectorPasswordlessExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreatePostgresServiceconnectorPasswordlessExtensionOptions(), token);
    }
}