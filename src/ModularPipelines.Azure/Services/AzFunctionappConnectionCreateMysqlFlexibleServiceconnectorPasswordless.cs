using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "connection", "create", "mysql-flexible")]
public class AzFunctionappConnectionCreateMysqlFlexibleServiceconnectorPasswordless
{
    public AzFunctionappConnectionCreateMysqlFlexibleServiceconnectorPasswordless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzFunctionappConnectionCreateMysqlFlexibleServiceconnectorPasswordlessExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateMysqlFlexibleServiceconnectorPasswordlessExtensionOptions(), token);
    }
}