using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql")]
public class AzSqlServer
{
    public AzSqlServer(
        AzSqlServerAdAdmin adAdmin,
        AzSqlServerAdOnlyAuth adOnlyAuth,
        AzSqlServerAdvancedThreatProtectionSetting advancedThreatProtectionSetting,
        AzSqlServerAuditPolicy auditPolicy,
        AzSqlServerConnPolicy connPolicy,
        AzSqlServerDnsAlias dnsAlias,
        AzSqlServerFirewallRule firewallRule,
        AzSqlServerIpv6FirewallRule ipv6FirewallRule,
        AzSqlServerKey key,
        AzSqlServerMsSupport msSupport,
        AzSqlServerOutboundFirewallRule outboundFirewallRule,
        AzSqlServerTdeKey tdeKey,
        AzSqlServerVnetRule vnetRule,
        ICommand internalCommand
    )
    {
        AdAdmin = adAdmin;
        AdOnlyAuth = adOnlyAuth;
        AdvancedThreatProtectionSetting = advancedThreatProtectionSetting;
        AuditPolicy = auditPolicy;
        ConnPolicy = connPolicy;
        DnsAlias = dnsAlias;
        FirewallRule = firewallRule;
        Ipv6FirewallRule = ipv6FirewallRule;
        Key = key;
        MsSupport = msSupport;
        OutboundFirewallRule = outboundFirewallRule;
        TdeKey = tdeKey;
        VnetRule = vnetRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSqlServerAdAdmin AdAdmin { get; }

    public AzSqlServerAdOnlyAuth AdOnlyAuth { get; }

    public AzSqlServerAdvancedThreatProtectionSetting AdvancedThreatProtectionSetting { get; }

    public AzSqlServerAuditPolicy AuditPolicy { get; }

    public AzSqlServerConnPolicy ConnPolicy { get; }

    public AzSqlServerDnsAlias DnsAlias { get; }

    public AzSqlServerFirewallRule FirewallRule { get; }

    public AzSqlServerIpv6FirewallRule Ipv6FirewallRule { get; }

    public AzSqlServerKey Key { get; }

    public AzSqlServerMsSupport MsSupport { get; }

    public AzSqlServerOutboundFirewallRule OutboundFirewallRule { get; }

    public AzSqlServerTdeKey TdeKey { get; }

    public AzSqlServerVnetRule VnetRule { get; }

    public async Task<CommandResult> Create(AzSqlServerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSqlServerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzSqlServerListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerListOptions(), token);
    }

    public async Task<CommandResult> ListUsages(AzSqlServerListUsagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerListUsagesOptions(), token);
    }

    public async Task<CommandResult> RefreshExternalGovernanceStatus(AzSqlServerRefreshExternalGovernanceStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerRefreshExternalGovernanceStatusOptions(), token);
    }

    public async Task<CommandResult> Show(AzSqlServerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlServerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzSqlServerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerWaitOptions(), token);
    }
}