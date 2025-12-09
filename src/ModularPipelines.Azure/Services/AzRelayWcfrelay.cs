using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("relay")]
public class AzRelayWcfrelay
{
    public AzRelayWcfrelay(
        AzRelayWcfrelayAuthorizationRule authorizationRule,
        ICommand internalCommand
    )
    {
        AuthorizationRule = authorizationRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzRelayWcfrelayAuthorizationRule AuthorizationRule { get; }

    public async Task<CommandResult> Create(AzRelayWcfrelayCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzRelayWcfrelayDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRelayWcfrelayDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzRelayWcfrelayListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzRelayWcfrelayShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRelayWcfrelayShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzRelayWcfrelayUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzRelayWcfrelayUpdateOptions(), token);
    }
}