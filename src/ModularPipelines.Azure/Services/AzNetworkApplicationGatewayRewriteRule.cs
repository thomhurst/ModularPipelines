using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway")]
public class AzNetworkApplicationGatewayRewriteRule
{
    public AzNetworkApplicationGatewayRewriteRule(
        AzNetworkApplicationGatewayRewriteRuleCondition condition,
        AzNetworkApplicationGatewayRewriteRuleSet set,
        ICommand internalCommand
    )
    {
        Condition = condition;
        Set = set;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkApplicationGatewayRewriteRuleCondition Condition { get; }

    public AzNetworkApplicationGatewayRewriteRuleSet Set { get; }

    public async Task<CommandResult> Create(AzNetworkApplicationGatewayRewriteRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkApplicationGatewayRewriteRuleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkApplicationGatewayRewriteRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRequestHeaders(AzNetworkApplicationGatewayRewriteRuleListRequestHeadersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayRewriteRuleListRequestHeadersOptions(), token);
    }

    public async Task<CommandResult> ListResponseHeaders(AzNetworkApplicationGatewayRewriteRuleListResponseHeadersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayRewriteRuleListResponseHeadersOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkApplicationGatewayRewriteRuleShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzNetworkApplicationGatewayRewriteRuleUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkApplicationGatewayRewriteRuleWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayRewriteRuleWaitOptions(), token);
    }
}