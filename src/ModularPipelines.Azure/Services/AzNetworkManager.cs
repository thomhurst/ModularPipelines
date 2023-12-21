using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkManager
{
    public AzNetworkManager(
        AzNetworkManagerConnectConfig connectConfig,
        AzNetworkManagerConnection connection,
        AzNetworkManagerGroup group,
        AzNetworkManagerScopeConnection scopeConnection,
        AzNetworkManagerSecurityAdminConfig securityAdminConfig,
        ICommand internalCommand
    )
    {
        ConnectConfig = connectConfig;
        Connection = connection;
        Group = group;
        ScopeConnection = scopeConnection;
        SecurityAdminConfig = securityAdminConfig;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkManagerConnectConfig ConnectConfig { get; }

    public AzNetworkManagerConnection Connection { get; }

    public AzNetworkManagerGroup Group { get; }

    public AzNetworkManagerScopeConnection ScopeConnection { get; }

    public AzNetworkManagerSecurityAdminConfig SecurityAdminConfig { get; }

    public async Task<CommandResult> Create(AzNetworkManagerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkManagerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkManagerListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerListOptions(), token);
    }

    public async Task<CommandResult> ListActiveConnectivityConfig(AzNetworkManagerListActiveConnectivityConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerListActiveConnectivityConfigOptions(), token);
    }

    public async Task<CommandResult> ListActiveSecurityAdminRule(AzNetworkManagerListActiveSecurityAdminRuleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerListActiveSecurityAdminRuleOptions(), token);
    }

    public async Task<CommandResult> ListDeployStatus(AzNetworkManagerListDeployStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerListDeployStatusOptions(), token);
    }

    public async Task<CommandResult> ListEffectiveConnectivityConfig(AzNetworkManagerListEffectiveConnectivityConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerListEffectiveConnectivityConfigOptions(), token);
    }

    public async Task<CommandResult> ListEffectiveSecurityAdminRule(AzNetworkManagerListEffectiveSecurityAdminRuleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerListEffectiveSecurityAdminRuleOptions(), token);
    }

    public async Task<CommandResult> PostCommit(AzNetworkManagerPostCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkManagerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkManagerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkManagerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkManagerWaitOptions(), token);
    }
}