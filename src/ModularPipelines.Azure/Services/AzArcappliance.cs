using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzArcappliance
{
    public AzArcappliance(
        AzArcapplianceCreate create,
        AzArcapplianceCreateconfig createconfig,
        AzArcapplianceDelete delete,
        AzArcapplianceDeploy deploy,
        AzArcapplianceLogs logs,
        AzArcappliancePrepare prepare,
        AzArcapplianceRun run,
        AzArcapplianceTroubleshoot troubleshoot,
        AzArcapplianceUpdateInfracredentials updateInfracredentials,
        AzArcapplianceUpgrade upgrade,
        AzArcapplianceValidate validate,
        ICommand internalCommand
    )
    {
        Create = create;
        Createconfig = createconfig;
        Delete = delete;
        Deploy = deploy;
        Logs = logs;
        Prepare = prepare;
        Run = run;
        Troubleshoot = troubleshoot;
        UpdateInfracredentials = updateInfracredentials;
        Upgrade = upgrade;
        Validate = validate;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzArcapplianceCreate Create { get; }

    public AzArcapplianceCreateconfig Createconfig { get; }

    public AzArcapplianceDelete Delete { get; }

    public AzArcapplianceDeploy Deploy { get; }

    public AzArcapplianceLogs Logs { get; }

    public AzArcappliancePrepare Prepare { get; }

    public AzArcapplianceRun Run { get; }

    public AzArcapplianceTroubleshoot Troubleshoot { get; }

    public AzArcapplianceUpdateInfracredentials UpdateInfracredentials { get; }

    public AzArcapplianceUpgrade Upgrade { get; }

    public AzArcapplianceValidate Validate { get; }

    public async Task<CommandResult> GetCredentials(AzArcapplianceGetCredentialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcapplianceGetCredentialsOptions(), token);
    }

    public async Task<CommandResult> GetUpgrades(AzArcapplianceGetUpgradesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzArcapplianceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcapplianceListOptions(), token);
    }

    public async Task<CommandResult> Notice(AzArcapplianceNoticeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcapplianceNoticeOptions(), token);
    }

    public async Task<CommandResult> Show(AzArcapplianceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}