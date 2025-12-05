using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network")]
public class AzMobileNetworkSim
{
    public AzMobileNetworkSim(
        AzMobileNetworkSimGroup group,
        AzMobileNetworkSimPolicy policy,
        ICommand internalCommand
    )
    {
        Group = group;
        Policy = policy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMobileNetworkSimGroup Group { get; }

    public AzMobileNetworkSimPolicy Policy { get; }

    public async Task<CommandResult> Create(AzMobileNetworkSimCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMobileNetworkSimDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMobileNetworkSimListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMobileNetworkSimShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMobileNetworkSimWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkSimWaitOptions(), token);
    }
}