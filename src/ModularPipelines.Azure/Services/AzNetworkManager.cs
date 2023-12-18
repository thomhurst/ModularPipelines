using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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

    public async Task<CommandResult> Delete(AzNetworkManagerDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkManagerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListActiveConnectivityConfig(AzNetworkManagerListActiveConnectivityConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListActiveSecurityAdminRule(AzNetworkManagerListActiveSecurityAdminRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDeployStatus(AzNetworkManagerListDeployStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEffectiveConnectivityConfig(AzNetworkManagerListEffectiveConnectivityConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEffectiveSecurityAdminRule(AzNetworkManagerListEffectiveSecurityAdminRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
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