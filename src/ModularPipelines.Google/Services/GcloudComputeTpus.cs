using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute")]
public class GcloudComputeTpus
{
    public GcloudComputeTpus(
        GcloudComputeTpusAcceleratorTypes acceleratorTypes,
        GcloudComputeTpusExecutionGroups executionGroups,
        GcloudComputeTpusLocations locations,
        GcloudComputeTpusTopologies topologies,
        GcloudComputeTpusTpuVm tpuVm,
        GcloudComputeTpusVersions versions,
        ICommand internalCommand
    )
    {
        AcceleratorTypes = acceleratorTypes;
        ExecutionGroups = executionGroups;
        Locations = locations;
        Topologies = topologies;
        TpuVm = tpuVm;
        Versions = versions;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeTpusAcceleratorTypes AcceleratorTypes { get; }

    public GcloudComputeTpusExecutionGroups ExecutionGroups { get; }

    public GcloudComputeTpusLocations Locations { get; }

    public GcloudComputeTpusTopologies Topologies { get; }

    public GcloudComputeTpusTpuVm TpuVm { get; }

    public GcloudComputeTpusVersions Versions { get; }

    public async Task<CommandResult> Create(GcloudComputeTpusCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComputeTpusDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputeTpusDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeTpusListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeTpusListOptions(), token);
    }

    public async Task<CommandResult> Reimage(GcloudComputeTpusReimageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(GcloudComputeTpusStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(GcloudComputeTpusStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}