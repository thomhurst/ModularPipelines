using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsChimeSdkMessaging
{
    public AwsChimeSdkMessaging(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateChannelFlow(AwsChimeSdkMessagingAssociateChannelFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchCreateChannelMembership(AwsChimeSdkMessagingBatchCreateChannelMembershipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ChannelFlowCallback(AwsChimeSdkMessagingChannelFlowCallbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateChannel(AwsChimeSdkMessagingCreateChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateChannelBan(AwsChimeSdkMessagingCreateChannelBanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateChannelFlow(AwsChimeSdkMessagingCreateChannelFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateChannelMembership(AwsChimeSdkMessagingCreateChannelMembershipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateChannelModerator(AwsChimeSdkMessagingCreateChannelModeratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteChannel(AwsChimeSdkMessagingDeleteChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteChannelBan(AwsChimeSdkMessagingDeleteChannelBanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteChannelFlow(AwsChimeSdkMessagingDeleteChannelFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteChannelMembership(AwsChimeSdkMessagingDeleteChannelMembershipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteChannelMessage(AwsChimeSdkMessagingDeleteChannelMessageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteChannelModerator(AwsChimeSdkMessagingDeleteChannelModeratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMessagingStreamingConfigurations(AwsChimeSdkMessagingDeleteMessagingStreamingConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeChannel(AwsChimeSdkMessagingDescribeChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeChannelBan(AwsChimeSdkMessagingDescribeChannelBanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeChannelFlow(AwsChimeSdkMessagingDescribeChannelFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeChannelMembership(AwsChimeSdkMessagingDescribeChannelMembershipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeChannelMembershipForAppInstanceUser(AwsChimeSdkMessagingDescribeChannelMembershipForAppInstanceUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeChannelModeratedByAppInstanceUser(AwsChimeSdkMessagingDescribeChannelModeratedByAppInstanceUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeChannelModerator(AwsChimeSdkMessagingDescribeChannelModeratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateChannelFlow(AwsChimeSdkMessagingDisassociateChannelFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetChannelMembershipPreferences(AwsChimeSdkMessagingGetChannelMembershipPreferencesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetChannelMessage(AwsChimeSdkMessagingGetChannelMessageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetChannelMessageStatus(AwsChimeSdkMessagingGetChannelMessageStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMessagingSessionEndpoint(AwsChimeSdkMessagingGetMessagingSessionEndpointOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkMessagingGetMessagingSessionEndpointOptions(), token);
    }

    public async Task<CommandResult> GetMessagingStreamingConfigurations(AwsChimeSdkMessagingGetMessagingStreamingConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChannelBans(AwsChimeSdkMessagingListChannelBansOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChannelFlows(AwsChimeSdkMessagingListChannelFlowsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChannelMemberships(AwsChimeSdkMessagingListChannelMembershipsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChannelMembershipsForAppInstanceUser(AwsChimeSdkMessagingListChannelMembershipsForAppInstanceUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChannelMessages(AwsChimeSdkMessagingListChannelMessagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChannelModerators(AwsChimeSdkMessagingListChannelModeratorsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChannels(AwsChimeSdkMessagingListChannelsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChannelsAssociatedWithChannelFlow(AwsChimeSdkMessagingListChannelsAssociatedWithChannelFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChannelsModeratedByAppInstanceUser(AwsChimeSdkMessagingListChannelsModeratedByAppInstanceUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSubChannels(AwsChimeSdkMessagingListSubChannelsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsChimeSdkMessagingListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutChannelExpirationSettings(AwsChimeSdkMessagingPutChannelExpirationSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutChannelMembershipPreferences(AwsChimeSdkMessagingPutChannelMembershipPreferencesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutMessagingStreamingConfigurations(AwsChimeSdkMessagingPutMessagingStreamingConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RedactChannelMessage(AwsChimeSdkMessagingRedactChannelMessageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchChannels(AwsChimeSdkMessagingSearchChannelsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendChannelMessage(AwsChimeSdkMessagingSendChannelMessageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsChimeSdkMessagingTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsChimeSdkMessagingUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateChannel(AwsChimeSdkMessagingUpdateChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateChannelFlow(AwsChimeSdkMessagingUpdateChannelFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateChannelMessage(AwsChimeSdkMessagingUpdateChannelMessageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateChannelReadMarker(AwsChimeSdkMessagingUpdateChannelReadMarkerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}