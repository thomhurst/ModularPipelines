using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp")]
public class AzContainerappEnv
{
    public AzContainerappEnv(
        AzContainerappEnvCertificate certificate,
        AzContainerappEnvDaprComponent daprComponent,
        AzContainerappEnvLogs logs,
        AzContainerappEnvStorage storage,
        AzContainerappEnvWorkloadProfile workloadProfile,
        ICommand internalCommand
    )
    {
        Certificate = certificate;
        DaprComponent = daprComponent;
        Logs = logs;
        Storage = storage;
        WorkloadProfile = workloadProfile;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzContainerappEnvCertificate Certificate { get; }

    public AzContainerappEnvDaprComponent DaprComponent { get; }

    public AzContainerappEnvLogs Logs { get; }

    public AzContainerappEnvStorage Storage { get; }

    public AzContainerappEnvWorkloadProfile WorkloadProfile { get; }

    public async Task<CommandResult> Create(AzContainerappEnvCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzContainerappEnvDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappEnvDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzContainerappEnvListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappEnvListOptions(), token);
    }

    public async Task<CommandResult> ListUsages(AzContainerappEnvListUsagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappEnvListUsagesOptions(), token);
    }

    public async Task<CommandResult> Show(AzContainerappEnvShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappEnvShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzContainerappEnvUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappEnvUpdateOptions(), token);
    }
}