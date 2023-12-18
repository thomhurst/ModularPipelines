using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus")]
public class AzServicebusGeorecoveryAlias
{
    public AzServicebusGeorecoveryAlias(
        AzServicebusGeorecoveryAliasAuthorizationRule authorizationRule,
        ICommand internalCommand
    )
    {
        AuthorizationRule = authorizationRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzServicebusGeorecoveryAliasAuthorizationRule AuthorizationRule { get; }

    public async Task<CommandResult> BreakPair(AzServicebusGeorecoveryAliasBreakPairOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzServicebusGeorecoveryAliasCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzServicebusGeorecoveryAliasDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzServicebusGeorecoveryAliasExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FailOver(AzServicebusGeorecoveryAliasFailOverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzServicebusGeorecoveryAliasListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzServicebusGeorecoveryAliasSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzServicebusGeorecoveryAliasShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusGeorecoveryAliasShowOptions(), token);
    }
}