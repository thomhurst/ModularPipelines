using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsPinpointSmsVoice
{
    public AwsPinpointSmsVoice(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateConfigurationSet(AwsPinpointSmsVoiceCreateConfigurationSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceCreateConfigurationSetOptions(), token);
    }

    public async Task<CommandResult> CreateConfigurationSetEventDestination(AwsPinpointSmsVoiceCreateConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationSet(AwsPinpointSmsVoiceDeleteConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationSetEventDestination(AwsPinpointSmsVoiceDeleteConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfigurationSetEventDestinations(AwsPinpointSmsVoiceGetConfigurationSetEventDestinationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendVoiceMessage(AwsPinpointSmsVoiceSendVoiceMessageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceSendVoiceMessageOptions(), token);
    }

    public async Task<CommandResult> UpdateConfigurationSetEventDestination(AwsPinpointSmsVoiceUpdateConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}