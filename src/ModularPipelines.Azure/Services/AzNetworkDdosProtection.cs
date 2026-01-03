using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network")]
public class AzNetworkDdosProtection
{
    public AzNetworkDdosProtection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkDdosProtectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkDdosProtectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDdosProtectionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkDdosProtectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDdosProtectionListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkDdosProtectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDdosProtectionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkDdosProtectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDdosProtectionUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkDdosProtectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkDdosProtectionWaitOptions(), cancellationToken: token);
    }
}