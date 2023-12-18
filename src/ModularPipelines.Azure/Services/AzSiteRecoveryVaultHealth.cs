using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "vault")]
public class AzSiteRecoveryVaultHealth
{
    public AzSiteRecoveryVaultHealth(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> RefreshDefault(AzSiteRecoveryVaultHealthRefreshDefaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryVaultHealthRefreshDefaultOptions(), token);
    }

    public async Task<CommandResult> Show(AzSiteRecoveryVaultHealthShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryVaultHealthShowOptions(), token);
    }
}