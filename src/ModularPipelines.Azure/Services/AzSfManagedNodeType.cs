using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf")]
public class AzSfManagedNodeType
{
    public AzSfManagedNodeType(
        AzSfManagedNodeTypeNode node,
        AzSfManagedNodeTypeVmExtension vmExtension,
        AzSfManagedNodeTypeVmSecret vmSecret,
        ICommand internalCommand
    )
    {
        Node = node;
        VmExtension = vmExtension;
        VmSecret = vmSecret;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSfManagedNodeTypeNode Node { get; }

    public AzSfManagedNodeTypeVmExtension VmExtension { get; }

    public AzSfManagedNodeTypeVmSecret VmSecret { get; }

    public async Task<CommandResult> Create(AzSfManagedNodeTypeCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSfManagedNodeTypeDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSfManagedNodeTypeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSfManagedNodeTypeShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSfManagedNodeTypeUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}