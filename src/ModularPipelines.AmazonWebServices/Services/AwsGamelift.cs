using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsGamelift
{
    public AwsGamelift(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptMatch(AwsGameliftAcceptMatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ClaimGameServer(AwsGameliftClaimGameServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAlias(AwsGameliftCreateAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBuild(AwsGameliftCreateBuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftCreateBuildOptions(), token);
    }

    public async Task<CommandResult> CreateFleet(AwsGameliftCreateFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFleetLocations(AwsGameliftCreateFleetLocationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGameServerGroup(AwsGameliftCreateGameServerGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGameSession(AwsGameliftCreateGameSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGameSessionQueue(AwsGameliftCreateGameSessionQueueOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLocation(AwsGameliftCreateLocationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMatchmakingConfiguration(AwsGameliftCreateMatchmakingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMatchmakingRuleSet(AwsGameliftCreateMatchmakingRuleSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePlayerSession(AwsGameliftCreatePlayerSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePlayerSessions(AwsGameliftCreatePlayerSessionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateScript(AwsGameliftCreateScriptOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftCreateScriptOptions(), token);
    }

    public async Task<CommandResult> CreateVpcPeeringAuthorization(AwsGameliftCreateVpcPeeringAuthorizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpcPeeringConnection(AwsGameliftCreateVpcPeeringConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAlias(AwsGameliftDeleteAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBuild(AwsGameliftDeleteBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFleet(AwsGameliftDeleteFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFleetLocations(AwsGameliftDeleteFleetLocationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGameServerGroup(AwsGameliftDeleteGameServerGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGameSessionQueue(AwsGameliftDeleteGameSessionQueueOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLocation(AwsGameliftDeleteLocationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMatchmakingConfiguration(AwsGameliftDeleteMatchmakingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMatchmakingRuleSet(AwsGameliftDeleteMatchmakingRuleSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteScalingPolicy(AwsGameliftDeleteScalingPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteScript(AwsGameliftDeleteScriptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpcPeeringAuthorization(AwsGameliftDeleteVpcPeeringAuthorizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpcPeeringConnection(AwsGameliftDeleteVpcPeeringConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterCompute(AwsGameliftDeregisterComputeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterGameServer(AwsGameliftDeregisterGameServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAlias(AwsGameliftDescribeAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBuild(AwsGameliftDescribeBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCompute(AwsGameliftDescribeComputeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEc2InstanceLimits(AwsGameliftDescribeEc2InstanceLimitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeEc2InstanceLimitsOptions(), token);
    }

    public async Task<CommandResult> DescribeFleetAttributes(AwsGameliftDescribeFleetAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeFleetAttributesOptions(), token);
    }

    public async Task<CommandResult> DescribeFleetCapacity(AwsGameliftDescribeFleetCapacityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeFleetCapacityOptions(), token);
    }

    public async Task<CommandResult> DescribeFleetEvents(AwsGameliftDescribeFleetEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFleetLocationAttributes(AwsGameliftDescribeFleetLocationAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFleetLocationCapacity(AwsGameliftDescribeFleetLocationCapacityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFleetLocationUtilization(AwsGameliftDescribeFleetLocationUtilizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFleetPortSettings(AwsGameliftDescribeFleetPortSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFleetUtilization(AwsGameliftDescribeFleetUtilizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeFleetUtilizationOptions(), token);
    }

    public async Task<CommandResult> DescribeGameServer(AwsGameliftDescribeGameServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeGameServerGroup(AwsGameliftDescribeGameServerGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeGameServerInstances(AwsGameliftDescribeGameServerInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeGameSessionDetails(AwsGameliftDescribeGameSessionDetailsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeGameSessionDetailsOptions(), token);
    }

    public async Task<CommandResult> DescribeGameSessionPlacement(AwsGameliftDescribeGameSessionPlacementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeGameSessionQueues(AwsGameliftDescribeGameSessionQueuesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeGameSessionQueuesOptions(), token);
    }

    public async Task<CommandResult> DescribeGameSessions(AwsGameliftDescribeGameSessionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeGameSessionsOptions(), token);
    }

    public async Task<CommandResult> DescribeInstances(AwsGameliftDescribeInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMatchmaking(AwsGameliftDescribeMatchmakingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMatchmakingConfigurations(AwsGameliftDescribeMatchmakingConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeMatchmakingConfigurationsOptions(), token);
    }

    public async Task<CommandResult> DescribeMatchmakingRuleSets(AwsGameliftDescribeMatchmakingRuleSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeMatchmakingRuleSetsOptions(), token);
    }

    public async Task<CommandResult> DescribePlayerSessions(AwsGameliftDescribePlayerSessionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribePlayerSessionsOptions(), token);
    }

    public async Task<CommandResult> DescribeRuntimeConfiguration(AwsGameliftDescribeRuntimeConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeScalingPolicies(AwsGameliftDescribeScalingPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeScript(AwsGameliftDescribeScriptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeVpcPeeringAuthorizations(AwsGameliftDescribeVpcPeeringAuthorizationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeVpcPeeringAuthorizationsOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcPeeringConnections(AwsGameliftDescribeVpcPeeringConnectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftDescribeVpcPeeringConnectionsOptions(), token);
    }

    public async Task<CommandResult> GetComputeAccess(AwsGameliftGetComputeAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetComputeAuthToken(AwsGameliftGetComputeAuthTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGameSessionLog(AwsGameliftGetGameSessionLogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGameSessionLogUrl(AwsGameliftGetGameSessionLogUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstanceAccess(AwsGameliftGetInstanceAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAliases(AwsGameliftListAliasesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftListAliasesOptions(), token);
    }

    public async Task<CommandResult> ListBuilds(AwsGameliftListBuildsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftListBuildsOptions(), token);
    }

    public async Task<CommandResult> ListCompute(AwsGameliftListComputeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFleets(AwsGameliftListFleetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftListFleetsOptions(), token);
    }

    public async Task<CommandResult> ListGameServerGroups(AwsGameliftListGameServerGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftListGameServerGroupsOptions(), token);
    }

    public async Task<CommandResult> ListGameServers(AwsGameliftListGameServersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLocations(AwsGameliftListLocationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftListLocationsOptions(), token);
    }

    public async Task<CommandResult> ListScripts(AwsGameliftListScriptsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftListScriptsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsGameliftListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutScalingPolicy(AwsGameliftPutScalingPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterCompute(AwsGameliftRegisterComputeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterGameServer(AwsGameliftRegisterGameServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RequestUploadCredentials(AwsGameliftRequestUploadCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResolveAlias(AwsGameliftResolveAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResumeGameServerGroup(AwsGameliftResumeGameServerGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchGameSessions(AwsGameliftSearchGameSessionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGameliftSearchGameSessionsOptions(), token);
    }

    public async Task<CommandResult> StartFleetActions(AwsGameliftStartFleetActionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartGameSessionPlacement(AwsGameliftStartGameSessionPlacementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMatchBackfill(AwsGameliftStartMatchBackfillOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMatchmaking(AwsGameliftStartMatchmakingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopFleetActions(AwsGameliftStopFleetActionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopGameSessionPlacement(AwsGameliftStopGameSessionPlacementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopMatchmaking(AwsGameliftStopMatchmakingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SuspendGameServerGroup(AwsGameliftSuspendGameServerGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsGameliftTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsGameliftUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAlias(AwsGameliftUpdateAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBuild(AwsGameliftUpdateBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFleetAttributes(AwsGameliftUpdateFleetAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFleetCapacity(AwsGameliftUpdateFleetCapacityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFleetPortSettings(AwsGameliftUpdateFleetPortSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGameServer(AwsGameliftUpdateGameServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGameServerGroup(AwsGameliftUpdateGameServerGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGameSession(AwsGameliftUpdateGameSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGameSessionQueue(AwsGameliftUpdateGameSessionQueueOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMatchmakingConfiguration(AwsGameliftUpdateMatchmakingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRuntimeConfiguration(AwsGameliftUpdateRuntimeConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateScript(AwsGameliftUpdateScriptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UploadBuild(AwsGameliftUploadBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ValidateMatchmakingRuleSet(AwsGameliftValidateMatchmakingRuleSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}