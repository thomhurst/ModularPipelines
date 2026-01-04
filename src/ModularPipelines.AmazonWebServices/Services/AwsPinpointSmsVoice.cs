using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CreateConfigurationSet(AwsPinpointSmsVoiceCreateConfigurationSetOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceCreateConfigurationSetOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateConfigurationSetEventDestination(AwsPinpointSmsVoiceCreateConfigurationSetEventDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConfigurationSet(AwsPinpointSmsVoiceDeleteConfigurationSetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConfigurationSetEventDestination(AwsPinpointSmsVoiceDeleteConfigurationSetEventDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConfigurationSetEventDestinations(AwsPinpointSmsVoiceGetConfigurationSetEventDestinationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SendVoiceMessage(AwsPinpointSmsVoiceSendVoiceMessageOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceSendVoiceMessageOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateConfigurationSetEventDestination(AwsPinpointSmsVoiceUpdateConfigurationSetEventDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}