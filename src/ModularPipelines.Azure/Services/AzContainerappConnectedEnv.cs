using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp")]
public class AzContainerappConnectedEnv
{
    public AzContainerappConnectedEnv(
        AzContainerappConnectedEnvCertificate certificate,
        AzContainerappConnectedEnvDaprComponent daprComponent,
        AzContainerappConnectedEnvStorage storage,
        ICommand internalCommand
    )
    {
        Certificate = certificate;
        DaprComponent = daprComponent;
        Storage = storage;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzContainerappConnectedEnvCertificate Certificate { get; }

    public AzContainerappConnectedEnvDaprComponent DaprComponent { get; }

    public AzContainerappConnectedEnvStorage Storage { get; }

    public async Task<CommandResult> Create(AzContainerappConnectedEnvCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzContainerappConnectedEnvDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectedEnvDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzContainerappConnectedEnvListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectedEnvListOptions(), token);
    }

    public async Task<CommandResult> Show(AzContainerappConnectedEnvShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectedEnvShowOptions(), token);
    }
}