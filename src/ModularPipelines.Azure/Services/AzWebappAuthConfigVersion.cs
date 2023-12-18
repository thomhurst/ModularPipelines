using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "auth")]
public class AzWebappAuthConfigVersion
{
    public AzWebappAuthConfigVersion(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Revert(AzWebappAuthConfigVersionRevertOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappAuthConfigVersionRevertOptions(), token);
    }

    public async Task<CommandResult> Show(AzWebappAuthConfigVersionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappAuthConfigVersionShowOptions(), token);
    }

    public async Task<CommandResult> Upgrade(AzWebappAuthConfigVersionUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappAuthConfigVersionUpgradeOptions(), token);
    }
}