using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver")]
public class AzDnsResolverForwardingRuleset
{
    public AzDnsResolverForwardingRuleset(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDnsResolverForwardingRulesetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDnsResolverForwardingRulesetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverForwardingRulesetDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDnsResolverForwardingRulesetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverForwardingRulesetListOptions(), token);
    }

    public async Task<CommandResult> ListByVirtualNetwork(AzDnsResolverForwardingRulesetListByVirtualNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDnsResolverForwardingRulesetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverForwardingRulesetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDnsResolverForwardingRulesetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverForwardingRulesetUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDnsResolverForwardingRulesetWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverForwardingRulesetWaitOptions(), token);
    }
}