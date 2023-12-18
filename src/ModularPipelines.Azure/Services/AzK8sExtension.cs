using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzK8sExtension
{
    public AzK8sExtension(
        AzK8sExtensionExtensionTypes extensionTypes,
        ICommand internalCommand
    )
    {
        ExtensionTypes = extensionTypes;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzK8sExtensionExtensionTypes ExtensionTypes { get; }

    public async Task<CommandResult> Create(AzK8sExtensionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzK8sExtensionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzK8sExtensionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzK8sExtensionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzK8sExtensionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

