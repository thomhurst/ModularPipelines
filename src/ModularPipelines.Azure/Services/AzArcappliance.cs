using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> GetCredentials(AzArcapplianceGetCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUpgrades(AzArcapplianceGetUpgradesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzArcapplianceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Notice(AzArcapplianceNoticeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzArcapplianceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}