using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs")]
public class AzEventhubsGeorecoveryAlias
{
    public AzEventhubsGeorecoveryAlias(
        AzEventhubsGeorecoveryAliasAuthorizationRule authorizationRule,
        ICommand internalCommand
    )
    {
        AuthorizationRule = authorizationRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventhubsGeorecoveryAliasAuthorizationRule AuthorizationRule { get; }

    public async Task<CommandResult> BreakPair(AzEventhubsGeorecoveryAliasBreakPairOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsGeorecoveryAliasBreakPairOptions(), token);
    }

    public async Task<CommandResult> Create(AzEventhubsGeorecoveryAliasCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventhubsGeorecoveryAliasDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsGeorecoveryAliasDeleteOptions(), token);
    }

    public async Task<CommandResult> Exists(AzEventhubsGeorecoveryAliasExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FailOver(AzEventhubsGeorecoveryAliasFailOverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsGeorecoveryAliasFailOverOptions(), token);
    }

    public async Task<CommandResult> List(AzEventhubsGeorecoveryAliasListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzEventhubsGeorecoveryAliasSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventhubsGeorecoveryAliasShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsGeorecoveryAliasShowOptions(), token);
    }
}