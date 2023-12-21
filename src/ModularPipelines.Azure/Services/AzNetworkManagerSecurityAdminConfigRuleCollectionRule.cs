using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "security-admin-config", "rule-collection")]
public class AzNetworkManagerSecurityAdminConfigRuleCollectionRule
{
    public AzNetworkManagerSecurityAdminConfigRuleCollectionRule(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkManagerSecurityAdminConfigRuleCollectionRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkManagerSecurityAdminConfigRuleCollectionRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerSecurityAdminConfigRuleCollectionRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkManagerSecurityAdminConfigRuleCollectionRuleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkManagerSecurityAdminConfigRuleCollectionRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerSecurityAdminConfigRuleCollectionRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkManagerSecurityAdminConfigRuleCollectionRuleUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}