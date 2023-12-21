using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "security-admin-config")]
public class AzNetworkManagerSecurityAdminConfigRuleCollection
{
    public AzNetworkManagerSecurityAdminConfigRuleCollection(
        AzNetworkManagerSecurityAdminConfigRuleCollectionRule rule,
        ICommand internalCommand
    )
    {
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkManagerSecurityAdminConfigRuleCollectionRule Rule { get; }

    public async Task<CommandResult> Create(AzNetworkManagerSecurityAdminConfigRuleCollectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkManagerSecurityAdminConfigRuleCollectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerSecurityAdminConfigRuleCollectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkManagerSecurityAdminConfigRuleCollectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkManagerSecurityAdminConfigRuleCollectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerSecurityAdminConfigRuleCollectionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkManagerSecurityAdminConfigRuleCollectionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkManagerSecurityAdminConfigRuleCollectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerSecurityAdminConfigRuleCollectionWaitOptions(), token);
    }
}