using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace")]
public class AzEventhubsNamespaceNetworkRuleSet
{
    public AzEventhubsNamespaceNetworkRuleSet(
        AzEventhubsNamespaceNetworkRuleSetIpRule ipRule,
        AzEventhubsNamespaceNetworkRuleSetVirtualNetworkRule virtualNetworkRule,
        ICommand internalCommand
    )
    {
        IpRule = ipRule;
        VirtualNetworkRule = virtualNetworkRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventhubsNamespaceNetworkRuleSetIpRule IpRule { get; }

    public AzEventhubsNamespaceNetworkRuleSetVirtualNetworkRule VirtualNetworkRule { get; }

    public async Task<CommandResult> Create(AzEventhubsNamespaceNetworkRuleSetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzEventhubsNamespaceNetworkRuleSetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventhubsNamespaceNetworkRuleSetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceNetworkRuleSetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventhubsNamespaceNetworkRuleSetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsNamespaceNetworkRuleSetUpdateOptions(), token);
    }
}