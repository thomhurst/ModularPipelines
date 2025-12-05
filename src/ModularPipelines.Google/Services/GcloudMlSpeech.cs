using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("ml")]
public class GcloudMlSpeech
{
    public GcloudMlSpeech(
        GcloudMlSpeechOperations operations,
        ICommand internalCommand
    )
    {
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudMlSpeechOperations Operations { get; }

    public async Task<CommandResult> Recognize(GcloudMlSpeechRecognizeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RecognizeLongRunning(GcloudMlSpeechRecognizeLongRunningOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}