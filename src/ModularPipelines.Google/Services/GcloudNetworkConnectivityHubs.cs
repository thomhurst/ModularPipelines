using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity")]
public class GcloudNetworkConnectivityHubs
{
    public GcloudNetworkConnectivityHubs(
        GcloudNetworkConnectivityHubsGroups groups,
        GcloudNetworkConnectivityHubsRouteTables routeTables,
        ICommand internalCommand
    )
    {
        Groups = groups;
        RouteTables = routeTables;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudNetworkConnectivityHubsGroups Groups { get; }

    public GcloudNetworkConnectivityHubsRouteTables RouteTables { get; }

    public async Task<CommandResult> AcceptSpoke(GcloudNetworkConnectivityHubsAcceptSpokeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddIamPolicyBinding(GcloudNetworkConnectivityHubsAddIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudNetworkConnectivityHubsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudNetworkConnectivityHubsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudNetworkConnectivityHubsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudNetworkConnectivityHubsGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudNetworkConnectivityHubsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudNetworkConnectivityHubsListOptions(), token);
    }

    public async Task<CommandResult> ListSpokes(GcloudNetworkConnectivityHubsListSpokesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectSpoke(GcloudNetworkConnectivityHubsRejectSpokeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveIamPolicyBinding(GcloudNetworkConnectivityHubsRemoveIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudNetworkConnectivityHubsSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudNetworkConnectivityHubsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}