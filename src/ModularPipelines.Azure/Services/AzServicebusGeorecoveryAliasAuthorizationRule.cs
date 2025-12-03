using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "georecovery-alias")]
public class AzServicebusGeorecoveryAliasAuthorizationRule
{
    public AzServicebusGeorecoveryAliasAuthorizationRule(
        AzServicebusGeorecoveryAliasAuthorizationRuleKeys keys,
        ICommand internalCommand
    )
    {
        Keys = keys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzServicebusGeorecoveryAliasAuthorizationRuleKeys Keys { get; }

    public async Task<CommandResult> List(AzServicebusGeorecoveryAliasAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzServicebusGeorecoveryAliasAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusGeorecoveryAliasAuthorizationRuleShowOptions(), token);
    }
}