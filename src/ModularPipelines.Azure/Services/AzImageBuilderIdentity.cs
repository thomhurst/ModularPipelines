using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("image", "builder")]
public class AzImageBuilderIdentity
{
    public AzImageBuilderIdentity(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Assign(AzImageBuilderIdentityAssignOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderIdentityAssignOptions(), token);
    }

    public async Task<CommandResult> Remove(AzImageBuilderIdentityRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderIdentityRemoveOptions(), token);
    }

    public async Task<CommandResult> Show(AzImageBuilderIdentityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderIdentityShowOptions(), token);
    }
}