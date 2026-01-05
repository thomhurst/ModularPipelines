using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CreateGroup(AwsResourceGroupsCreateGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteGroup(AwsResourceGroupsDeleteGroupOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsDeleteGroupOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAccountSettings(AwsResourceGroupsGetAccountSettingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsGetAccountSettingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetGroup(AwsResourceGroupsGetGroupOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsGetGroupOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetGroupConfiguration(AwsResourceGroupsGetGroupConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsGetGroupConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetGroupQuery(AwsResourceGroupsGetGroupQueryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsGetGroupQueryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTags(AwsResourceGroupsGetTagsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GroupResources(AwsResourceGroupsGroupResourcesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListGroupResources(AwsResourceGroupsListGroupResourcesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsListGroupResourcesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListGroups(AwsResourceGroupsListGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsListGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutGroupConfiguration(AwsResourceGroupsPutGroupConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsPutGroupConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchResources(AwsResourceGroupsSearchResourcesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> Tag(AwsResourceGroupsTagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UngroupResources(AwsResourceGroupsUngroupResourcesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> Untag(AwsResourceGroupsUntagOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateAccountSettings(AwsResourceGroupsUpdateAccountSettingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsUpdateAccountSettingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateGroup(AwsResourceGroupsUpdateGroupOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceGroupsUpdateGroupOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateGroupQuery(AwsResourceGroupsUpdateGroupQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}