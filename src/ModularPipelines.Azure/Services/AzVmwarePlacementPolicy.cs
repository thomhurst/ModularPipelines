using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware")]
public class AzVmwarePlacementPolicy
{
    public AzVmwarePlacementPolicy(
        AzVmwarePlacementPolicyVm vm,
        AzVmwarePlacementPolicyVmHost vmHost,
        ICommand internalCommand
    )
    {
        Vm = vm;
        VmHost = vmHost;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzVmwarePlacementPolicyVm Vm { get; }

    public AzVmwarePlacementPolicyVmHost VmHost { get; }

    public async Task<CommandResult> List(AzVmwarePlacementPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzVmwarePlacementPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwarePlacementPolicyShowOptions(), token);
    }
}