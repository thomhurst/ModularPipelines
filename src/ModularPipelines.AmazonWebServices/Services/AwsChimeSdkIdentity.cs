using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsChimeSdkIdentity
{
    public AwsChimeSdkIdentity(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateAppInstance(AwsChimeSdkIdentityCreateAppInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAppInstanceAdmin(AwsChimeSdkIdentityCreateAppInstanceAdminOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAppInstanceBot(AwsChimeSdkIdentityCreateAppInstanceBotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAppInstanceUser(AwsChimeSdkIdentityCreateAppInstanceUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppInstance(AwsChimeSdkIdentityDeleteAppInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppInstanceAdmin(AwsChimeSdkIdentityDeleteAppInstanceAdminOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppInstanceBot(AwsChimeSdkIdentityDeleteAppInstanceBotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppInstanceUser(AwsChimeSdkIdentityDeleteAppInstanceUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterAppInstanceUserEndpoint(AwsChimeSdkIdentityDeregisterAppInstanceUserEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppInstance(AwsChimeSdkIdentityDescribeAppInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppInstanceAdmin(AwsChimeSdkIdentityDescribeAppInstanceAdminOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppInstanceBot(AwsChimeSdkIdentityDescribeAppInstanceBotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppInstanceUser(AwsChimeSdkIdentityDescribeAppInstanceUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppInstanceUserEndpoint(AwsChimeSdkIdentityDescribeAppInstanceUserEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAppInstanceRetentionSettings(AwsChimeSdkIdentityGetAppInstanceRetentionSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppInstanceAdmins(AwsChimeSdkIdentityListAppInstanceAdminsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppInstanceBots(AwsChimeSdkIdentityListAppInstanceBotsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppInstanceUserEndpoints(AwsChimeSdkIdentityListAppInstanceUserEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppInstanceUsers(AwsChimeSdkIdentityListAppInstanceUsersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppInstances(AwsChimeSdkIdentityListAppInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkIdentityListAppInstancesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsChimeSdkIdentityListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAppInstanceRetentionSettings(AwsChimeSdkIdentityPutAppInstanceRetentionSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAppInstanceUserExpirationSettings(AwsChimeSdkIdentityPutAppInstanceUserExpirationSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterAppInstanceUserEndpoint(AwsChimeSdkIdentityRegisterAppInstanceUserEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsChimeSdkIdentityTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsChimeSdkIdentityUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAppInstance(AwsChimeSdkIdentityUpdateAppInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAppInstanceBot(AwsChimeSdkIdentityUpdateAppInstanceBotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAppInstanceUser(AwsChimeSdkIdentityUpdateAppInstanceUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAppInstanceUserEndpoint(AwsChimeSdkIdentityUpdateAppInstanceUserEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}