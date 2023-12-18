using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "config", "container", "set")]
public class AzFunctionappConfigContainerSetAppserviceKube
{
    public AzFunctionappConfigContainerSetAppserviceKube(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzFunctionappConfigContainerSetAppserviceKubeExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConfigContainerSetAppserviceKubeExtensionOptions(), token);
    }
}