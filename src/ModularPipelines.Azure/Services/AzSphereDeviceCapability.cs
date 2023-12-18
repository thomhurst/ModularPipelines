using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device")]
public class AzSphereDeviceCapability
{
    public AzSphereDeviceCapability(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Download(AzSphereDeviceCapabilityDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Select(AzSphereDeviceCapabilitySelectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceCapabilitySelectOptions(), token);
    }

    public async Task<CommandResult> ShowAttached(AzSphereDeviceCapabilityShowAttachedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSphereDeviceCapabilityShowAttachedOptions(), token);
    }

    public async Task<CommandResult> Update(AzSphereDeviceCapabilityUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}