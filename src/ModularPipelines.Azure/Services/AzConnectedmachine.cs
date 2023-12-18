using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzConnectedmachine
{
    public AzConnectedmachine(
        AzConnectedmachineExtension extension,
        AzConnectedmachinePrivateEndpointConnection privateEndpointConnection,
        AzConnectedmachinePrivateLinkResource privateLinkResource,
        AzConnectedmachinePrivateLinkScope privateLinkScope,
        AzConnectedmachineRunCommand runCommand,
        ICommand internalCommand
    )
    {
        Extension = extension;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        PrivateLinkScope = privateLinkScope;
        RunCommand = runCommand;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzConnectedmachineExtension Extension { get; }

    public AzConnectedmachinePrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzConnectedmachinePrivateLinkResource PrivateLinkResource { get; }

    public AzConnectedmachinePrivateLinkScope PrivateLinkScope { get; }

    public AzConnectedmachineRunCommand RunCommand { get; }

    public async Task<CommandResult> AssessPatches(AzConnectedmachineAssessPatchesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineAssessPatchesOptions(), token);
    }

    public async Task<CommandResult> Delete(AzConnectedmachineDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineDeleteOptions(), token);
    }

    public async Task<CommandResult> InstallPatches(AzConnectedmachineInstallPatchesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzConnectedmachineListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzConnectedmachineShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzConnectedmachineUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineUpdateOptions(), token);
    }

    public async Task<CommandResult> UpgradeExtension(AzConnectedmachineUpgradeExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachineUpgradeExtensionOptions(), token);
    }
}