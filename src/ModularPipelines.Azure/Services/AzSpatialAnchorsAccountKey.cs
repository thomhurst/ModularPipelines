using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spatial-anchors-account")]
public class AzSpatialAnchorsAccountKey
{
    public AzSpatialAnchorsAccountKey(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Renew(AzSpatialAnchorsAccountKeyRenewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpatialAnchorsAccountKeyRenewOptions(), token);
    }

    public async Task<CommandResult> Show(AzSpatialAnchorsAccountKeyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpatialAnchorsAccountKeyShowOptions(), token);
    }
}

