using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("relay", "hyco")]
public class AzRelayHycoAuthorizationRule
{
    public AzRelayHycoAuthorizationRule(
        AzRelayHycoAuthorizationRuleKeys keys,
        ICommand internalCommand
    )
    {
        Keys = keys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzRelayHycoAuthorizationRuleKeys Keys { get; }

    public async Task<CommandResult> Create(AzRelayHycoAuthorizationRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzRelayHycoAuthorizationRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRelayHycoAuthorizationRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzRelayHycoAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzRelayHycoAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRelayHycoAuthorizationRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzRelayHycoAuthorizationRuleUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}