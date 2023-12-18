using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "georecovery-alias")]
public class AzEventhubsGeorecoveryAliasAuthorizationRule
{
    public AzEventhubsGeorecoveryAliasAuthorizationRule(
        AzEventhubsGeorecoveryAliasAuthorizationRuleKeys keys,
        ICommand internalCommand
    )
    {
        Keys = keys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventhubsGeorecoveryAliasAuthorizationRuleKeys Keys { get; }

    public async Task<CommandResult> List(AzEventhubsGeorecoveryAliasAuthorizationRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEventhubsGeorecoveryAliasAuthorizationRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsGeorecoveryAliasAuthorizationRuleShowOptions(), token);
    }
}