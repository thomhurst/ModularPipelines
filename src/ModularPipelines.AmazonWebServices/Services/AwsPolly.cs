using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsPolly
{
    public AwsPolly(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DeleteLexicon(AwsPollyDeleteLexiconOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeVoices(AwsPollyDescribeVoicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPollyDescribeVoicesOptions(), token);
    }

    public async Task<CommandResult> GetLexicon(AwsPollyGetLexiconOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSpeechSynthesisTask(AwsPollyGetSpeechSynthesisTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLexicons(AwsPollyListLexiconsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPollyListLexiconsOptions(), token);
    }

    public async Task<CommandResult> ListSpeechSynthesisTasks(AwsPollyListSpeechSynthesisTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPollyListSpeechSynthesisTasksOptions(), token);
    }

    public async Task<CommandResult> PutLexicon(AwsPollyPutLexiconOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSpeechSynthesisTask(AwsPollyStartSpeechSynthesisTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SynthesizeSpeech(AwsPollySynthesizeSpeechOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}