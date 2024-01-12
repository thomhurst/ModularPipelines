using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances")]
public class GcloudComputeInstancesOsInventory
{
    public GcloudComputeInstancesOsInventory(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudComputeInstancesOsInventoryDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInstances(GcloudComputeInstancesOsInventoryListInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeInstancesOsInventoryListInstancesOptions(), token);
    }
}