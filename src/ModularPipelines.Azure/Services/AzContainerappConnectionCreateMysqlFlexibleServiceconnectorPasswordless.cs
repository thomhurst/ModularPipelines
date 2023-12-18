using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connection", "create", "mysql-flexible")]
public class AzContainerappConnectionCreateMysqlFlexibleServiceconnectorPasswordless
{
    public AzContainerappConnectionCreateMysqlFlexibleServiceconnectorPasswordless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzContainerappConnectionCreateMysqlFlexibleServiceconnectorPasswordlessExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateMysqlFlexibleServiceconnectorPasswordlessExtensionOptions(), token);
    }
}