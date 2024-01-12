using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml")]
public class GcloudMlVideo
{
    public GcloudMlVideo(
        GcloudMlVideoOperations operations,
        ICommand internalCommand
    )
    {
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudMlVideoOperations Operations { get; }

    public async Task<CommandResult> DetectExplicitContent(GcloudMlVideoDetectExplicitContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectLabels(GcloudMlVideoDetectLabelsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectShotChanges(GcloudMlVideoDetectShotChangesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}