using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis")]
public class AzRedisIdentity
{
    public AzRedisIdentity(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Assign(AzRedisIdentityAssignOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisIdentityAssignOptions(), token);
    }

    public async Task<CommandResult> Remove(AzRedisIdentityRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisIdentityRemoveOptions(), token);
    }

    public async Task<CommandResult> Show(AzRedisIdentityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRedisIdentityShowOptions(), token);
    }
}