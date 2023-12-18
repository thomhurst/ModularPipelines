using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "network-rule-set")]
public class AzEventhubsNamespaceNetworkRuleSetVirtualNetworkRule
{
    public AzEventhubsNamespaceNetworkRuleSetVirtualNetworkRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzEventhubsNamespaceNetworkRuleSetVirtualNetworkRuleAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzEventhubsNamespaceNetworkRuleSetVirtualNetworkRuleRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}