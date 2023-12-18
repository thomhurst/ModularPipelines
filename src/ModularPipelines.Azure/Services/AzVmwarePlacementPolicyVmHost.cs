using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "placement-policy")]
public class AzVmwarePlacementPolicyVmHost
{
    public AzVmwarePlacementPolicyVmHost(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzVmwarePlacementPolicyVmHostCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzVmwarePlacementPolicyVmHostDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwarePlacementPolicyVmHostDeleteOptions(), token);
    }

    public async Task<CommandResult> Update(AzVmwarePlacementPolicyVmHostUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwarePlacementPolicyVmHostUpdateOptions(), token);
    }
}