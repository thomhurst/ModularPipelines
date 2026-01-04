using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsResourceExplorer2
{
    public AwsResourceExplorer2(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateDefaultView(AwsResourceExplorer2AssociateDefaultViewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetView(AwsResourceExplorer2BatchGetViewOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2BatchGetViewOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateIndex(AwsResourceExplorer2CreateIndexOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2CreateIndexOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateView(AwsResourceExplorer2CreateViewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteIndex(AwsResourceExplorer2DeleteIndexOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteView(AwsResourceExplorer2DeleteViewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateDefaultView(AwsResourceExplorer2DisassociateDefaultViewOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2DisassociateDefaultViewOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAccountLevelServiceConfiguration(AwsResourceExplorer2GetAccountLevelServiceConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2GetAccountLevelServiceConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDefaultView(AwsResourceExplorer2GetDefaultViewOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2GetDefaultViewOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetIndex(AwsResourceExplorer2GetIndexOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2GetIndexOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetView(AwsResourceExplorer2GetViewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListIndexes(AwsResourceExplorer2ListIndexesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2ListIndexesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListIndexesForMembers(AwsResourceExplorer2ListIndexesForMembersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSupportedResourceTypes(AwsResourceExplorer2ListSupportedResourceTypesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2ListSupportedResourceTypesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsResourceExplorer2ListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListViews(AwsResourceExplorer2ListViewsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2ListViewsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> Search(AwsResourceExplorer2SearchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsResourceExplorer2TagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsResourceExplorer2UntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateIndexType(AwsResourceExplorer2UpdateIndexTypeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateView(AwsResourceExplorer2UpdateViewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}