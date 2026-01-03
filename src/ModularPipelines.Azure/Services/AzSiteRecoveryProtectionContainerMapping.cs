using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("site-recovery", "protection-container")]
public class AzSiteRecoveryProtectionContainerMapping
{
    public AzSiteRecoveryProtectionContainerMapping(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzSiteRecoveryProtectionContainerMappingCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzSiteRecoveryProtectionContainerMappingDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectionContainerMappingDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzSiteRecoveryProtectionContainerMappingListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Remove(AzSiteRecoveryProtectionContainerMappingRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectionContainerMappingRemoveOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzSiteRecoveryProtectionContainerMappingShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectionContainerMappingShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzSiteRecoveryProtectionContainerMappingUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectionContainerMappingUpdateOptions(), cancellationToken: token);
    }
}