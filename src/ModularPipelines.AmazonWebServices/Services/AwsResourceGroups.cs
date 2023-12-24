using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsResourceGroups
{
    public AwsResourceGroups(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateGroup(AwsResourceGroupsCreateGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGroup(AwsResourceGroupsDeleteGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsDeleteGroupOptions(), token);
    }

    public async Task<CommandResult> GetAccountSettings(AwsResourceGroupsGetAccountSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsGetAccountSettingsOptions(), token);
    }

    public async Task<CommandResult> GetGroup(AwsResourceGroupsGetGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsGetGroupOptions(), token);
    }

    public async Task<CommandResult> GetGroupConfiguration(AwsResourceGroupsGetGroupConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsGetGroupConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetGroupQuery(AwsResourceGroupsGetGroupQueryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsGetGroupQueryOptions(), token);
    }

    public async Task<CommandResult> GetTags(AwsResourceGroupsGetTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GroupResources(AwsResourceGroupsGroupResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListGroupResources(AwsResourceGroupsListGroupResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsListGroupResourcesOptions(), token);
    }

    public async Task<CommandResult> ListGroups(AwsResourceGroupsListGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsListGroupsOptions(), token);
    }

    public async Task<CommandResult> PutGroupConfiguration(AwsResourceGroupsPutGroupConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsPutGroupConfigurationOptions(), token);
    }

    public async Task<CommandResult> SearchResources(AwsResourceGroupsSearchResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Tag(AwsResourceGroupsTagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UngroupResources(AwsResourceGroupsUngroupResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Untag(AwsResourceGroupsUntagOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAccountSettings(AwsResourceGroupsUpdateAccountSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsUpdateAccountSettingsOptions(), token);
    }

    public async Task<CommandResult> UpdateGroup(AwsResourceGroupsUpdateGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsUpdateGroupOptions(), token);
    }

    public async Task<CommandResult> UpdateGroupQuery(AwsResourceGroupsUpdateGroupQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}