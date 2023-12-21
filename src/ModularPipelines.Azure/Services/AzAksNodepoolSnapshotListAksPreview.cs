using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "snapshot", "list")]
public class AzAksNodepoolSnapshotListAksPreview
{
    public AzAksNodepoolSnapshotListAksPreview(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzAksNodepoolSnapshotListAksPreviewExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAksNodepoolSnapshotListAksPreviewExtensionOptions(), token);
    }
}