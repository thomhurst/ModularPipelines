using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsIvsRealtime
{
    public AwsIvsRealtime(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateEncoderConfiguration(AwsIvsRealtimeCreateEncoderConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIvsRealtimeCreateEncoderConfigurationOptions(), token);
    }

    public async Task<CommandResult> CreateParticipantToken(AwsIvsRealtimeCreateParticipantTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStage(AwsIvsRealtimeCreateStageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIvsRealtimeCreateStageOptions(), token);
    }

    public async Task<CommandResult> CreateStorageConfiguration(AwsIvsRealtimeCreateStorageConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEncoderConfiguration(AwsIvsRealtimeDeleteEncoderConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStage(AwsIvsRealtimeDeleteStageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStorageConfiguration(AwsIvsRealtimeDeleteStorageConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisconnectParticipant(AwsIvsRealtimeDisconnectParticipantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetComposition(AwsIvsRealtimeGetCompositionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEncoderConfiguration(AwsIvsRealtimeGetEncoderConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetParticipant(AwsIvsRealtimeGetParticipantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStage(AwsIvsRealtimeGetStageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStageSession(AwsIvsRealtimeGetStageSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStorageConfiguration(AwsIvsRealtimeGetStorageConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCompositions(AwsIvsRealtimeListCompositionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIvsRealtimeListCompositionsOptions(), token);
    }

    public async Task<CommandResult> ListEncoderConfigurations(AwsIvsRealtimeListEncoderConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIvsRealtimeListEncoderConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListParticipantEvents(AwsIvsRealtimeListParticipantEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListParticipants(AwsIvsRealtimeListParticipantsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStageSessions(AwsIvsRealtimeListStageSessionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStages(AwsIvsRealtimeListStagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIvsRealtimeListStagesOptions(), token);
    }

    public async Task<CommandResult> ListStorageConfigurations(AwsIvsRealtimeListStorageConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIvsRealtimeListStorageConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsIvsRealtimeListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartComposition(AwsIvsRealtimeStartCompositionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopComposition(AwsIvsRealtimeStopCompositionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsIvsRealtimeTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsIvsRealtimeUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStage(AwsIvsRealtimeUpdateStageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}