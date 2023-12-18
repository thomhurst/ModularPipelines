using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkSecurityPartnerProvider
{
    public AzNetworkSecurityPartnerProvider(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkSecurityPartnerProviderCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkSecurityPartnerProviderDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkSecurityPartnerProviderDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkSecurityPartnerProviderListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkSecurityPartnerProviderListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkSecurityPartnerProviderShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkSecurityPartnerProviderShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkSecurityPartnerProviderUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkSecurityPartnerProviderUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkSecurityPartnerProviderWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkSecurityPartnerProviderWaitOptions(), token);
    }
}