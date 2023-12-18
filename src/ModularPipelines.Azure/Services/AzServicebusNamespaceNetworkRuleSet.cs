using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "namespace")]
public class AzServicebusNamespaceNetworkRuleSet
{
    public AzServicebusNamespaceNetworkRuleSet(
        AzServicebusNamespaceNetworkRuleSetIpRule ipRule,
        AzServicebusNamespaceNetworkRuleSetVirtualNetworkRule virtualNetworkRule,
        ICommand internalCommand
    )
    {
        IpRule = ipRule;
        VirtualNetworkRule = virtualNetworkRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzServicebusNamespaceNetworkRuleSetIpRule IpRule { get; }

    public AzServicebusNamespaceNetworkRuleSetVirtualNetworkRule VirtualNetworkRule { get; }

    public async Task<CommandResult> Create(AzServicebusNamespaceNetworkRuleSetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzServicebusNamespaceNetworkRuleSetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzServicebusNamespaceNetworkRuleSetShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespaceNetworkRuleSetShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzServicebusNamespaceNetworkRuleSetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespaceNetworkRuleSetUpdateOptions(), token);
    }
}