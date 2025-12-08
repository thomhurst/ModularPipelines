using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("compute")]
public class GcloudComputeInstanceGroups
{
    public GcloudComputeInstanceGroups(
        GcloudComputeInstanceGroupsManaged managed,
        GcloudComputeInstanceGroupsUnmanaged unmanaged,
        ICommand internalCommand
    )
    {
        Managed = managed;
        Unmanaged = unmanaged;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeInstanceGroupsManaged Managed { get; }

    public GcloudComputeInstanceGroupsUnmanaged Unmanaged { get; }

    public async Task<CommandResult> Describe(GcloudComputeInstanceGroupsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNamedPorts(GcloudComputeInstanceGroupsGetNamedPortsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeInstanceGroupsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInstances(GcloudComputeInstanceGroupsListInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetNamedPorts(GcloudComputeInstanceGroupsSetNamedPortsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}