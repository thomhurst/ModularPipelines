using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager")]
public class AzNetworkManagerSecurityAdminConfig
{
    public AzNetworkManagerSecurityAdminConfig(
        AzNetworkManagerSecurityAdminConfigRuleCollection ruleCollection,
        ICommand internalCommand
    )
    {
        RuleCollection = ruleCollection;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkManagerSecurityAdminConfigRuleCollection RuleCollection { get; }

    public async Task<CommandResult> Create(AzNetworkManagerSecurityAdminConfigCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkManagerSecurityAdminConfigDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkManagerSecurityAdminConfigListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkManagerSecurityAdminConfigShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerSecurityAdminConfigShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkManagerSecurityAdminConfigUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerSecurityAdminConfigUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkManagerSecurityAdminConfigWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerSecurityAdminConfigWaitOptions(), token);
    }
}