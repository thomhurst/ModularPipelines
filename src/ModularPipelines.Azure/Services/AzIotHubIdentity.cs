using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub")]
public class AzIotHubIdentity
{
    public AzIotHubIdentity(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Assign(AzIotHubIdentityAssignOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotHubIdentityAssignOptions(), token);
    }

    public async Task<CommandResult> Remove(AzIotHubIdentityRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotHubIdentityRemoveOptions(), token);
    }

    public async Task<CommandResult> Show(AzIotHubIdentityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotHubIdentityShowOptions(), token);
    }
}