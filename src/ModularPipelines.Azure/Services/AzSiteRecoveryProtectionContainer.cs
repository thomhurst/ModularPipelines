using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery")]
public class AzSiteRecoveryProtectionContainer
{
    public AzSiteRecoveryProtectionContainer(
        AzSiteRecoveryProtectionContainerMapping mapping,
        ICommand internalCommand
    )
    {
        Mapping = mapping;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSiteRecoveryProtectionContainerMapping Mapping { get; }

    public async Task<CommandResult> Create(AzSiteRecoveryProtectionContainerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSiteRecoveryProtectionContainerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzSiteRecoveryProtectionContainerRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectionContainerRemoveOptions(), token);
    }

    public async Task<CommandResult> Show(AzSiteRecoveryProtectionContainerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectionContainerShowOptions(), token);
    }

    public async Task<CommandResult> SwitchProtection(AzSiteRecoveryProtectionContainerSwitchProtectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectionContainerSwitchProtectionOptions(), token);
    }

    public async Task<CommandResult> Update(AzSiteRecoveryProtectionContainerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectionContainerUpdateOptions(), token);
    }
}

