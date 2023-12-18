using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "endpoint")]
public class AzCdnEndpointRule
{
    public AzCdnEndpointRule(
        AzCdnEndpointRuleAction action,
        AzCdnEndpointRuleCondition condition,
        ICommand internalCommand
    )
    {
        Action = action;
        Condition = condition;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCdnEndpointRuleAction Action { get; }

    public AzCdnEndpointRuleCondition Condition { get; }

    public async Task<CommandResult> Add(AzCdnEndpointRuleAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzCdnEndpointRuleRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnEndpointRuleRemoveOptions(), token);
    }

    public async Task<CommandResult> Show(AzCdnEndpointRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnEndpointRuleShowOptions(), token);
    }
}