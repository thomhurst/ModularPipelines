using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql")]
public class AzSqlMi
{
    public AzSqlMi(
        AzSqlMiAdAdmin adAdmin,
        AzSqlMiAdOnlyAuth adOnlyAuth,
        AzSqlMiAdvancedThreatProtectionSetting advancedThreatProtectionSetting,
        AzSqlMiDtc dtc,
        AzSqlMiEndpointCert endpointCert,
        AzSqlMiKey key,
        AzSqlMiLink link,
        AzSqlMiOp op,
        AzSqlMiPartnerCert partnerCert,
        AzSqlMiServerConfigurationOption serverConfigurationOption,
        AzSqlMiStartStopSchedule startStopSchedule,
        AzSqlMiTdeKey tdeKey,
        ICommand internalCommand
    )
    {
        AdAdmin = adAdmin;
        AdOnlyAuth = adOnlyAuth;
        AdvancedThreatProtectionSetting = advancedThreatProtectionSetting;
        Dtc = dtc;
        EndpointCert = endpointCert;
        Key = key;
        Link = link;
        Op = op;
        PartnerCert = partnerCert;
        ServerConfigurationOption = serverConfigurationOption;
        StartStopSchedule = startStopSchedule;
        TdeKey = tdeKey;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSqlMiAdAdmin AdAdmin { get; }

    public AzSqlMiAdOnlyAuth AdOnlyAuth { get; }

    public AzSqlMiAdvancedThreatProtectionSetting AdvancedThreatProtectionSetting { get; }

    public AzSqlMiDtc Dtc { get; }

    public AzSqlMiEndpointCert EndpointCert { get; }

    public AzSqlMiKey Key { get; }

    public AzSqlMiLink Link { get; }

    public AzSqlMiOp Op { get; }

    public AzSqlMiPartnerCert PartnerCert { get; }

    public AzSqlMiServerConfigurationOption ServerConfigurationOption { get; }

    public AzSqlMiStartStopSchedule StartStopSchedule { get; }

    public AzSqlMiTdeKey TdeKey { get; }

    public async Task<CommandResult> Create(AzSqlMiCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSqlMiDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiDeleteOptions(), token);
    }

    public async Task<CommandResult> Failover(AzSqlMiFailoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiFailoverOptions(), token);
    }

    public async Task<CommandResult> List(AzSqlMiListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiListOptions(), token);
    }

    public async Task<CommandResult> Show(AzSqlMiShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzSqlMiStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzSqlMiStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlMiUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiUpdateOptions(), token);
    }
}