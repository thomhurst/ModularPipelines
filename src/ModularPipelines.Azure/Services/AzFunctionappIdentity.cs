using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp")]
public class AzFunctionappIdentity
{
    public AzFunctionappIdentity(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Assign(AzFunctionappIdentityAssignOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappIdentityAssignOptions(), token);
    }

    public async Task<CommandResult> Remove(AzFunctionappIdentityRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappIdentityRemoveOptions(), token);
    }

    public async Task<CommandResult> Show(AzFunctionappIdentityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappIdentityShowOptions(), token);
    }
}