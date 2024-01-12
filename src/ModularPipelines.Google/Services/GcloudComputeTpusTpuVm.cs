using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus")]
public class GcloudComputeTpusTpuVm
{
    public GcloudComputeTpusTpuVm(
        GcloudComputeTpusTpuVmAcceleratorTypes acceleratorTypes,
        GcloudComputeTpusTpuVmServiceIdentity serviceIdentity,
        GcloudComputeTpusTpuVmVersions versions,
        ICommand internalCommand
    )
    {
        AcceleratorTypes = acceleratorTypes;
        ServiceIdentity = serviceIdentity;
        Versions = versions;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeTpusTpuVmAcceleratorTypes AcceleratorTypes { get; }

    public GcloudComputeTpusTpuVmServiceIdentity ServiceIdentity { get; }

    public GcloudComputeTpusTpuVmVersions Versions { get; }

    public async Task<CommandResult> Create(GcloudComputeTpusTpuVmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComputeTpusTpuVmDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputeTpusTpuVmDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGuestAttributes(GcloudComputeTpusTpuVmGetGuestAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeTpusTpuVmListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeTpusTpuVmListOptions(), token);
    }

    public async Task<CommandResult> Scp(GcloudComputeTpusTpuVmScpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Ssh(GcloudComputeTpusTpuVmSshOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(GcloudComputeTpusTpuVmStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(GcloudComputeTpusTpuVmStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudComputeTpusTpuVmUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}