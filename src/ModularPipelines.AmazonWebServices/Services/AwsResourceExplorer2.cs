using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> AssociateDefaultView(AwsResourceExplorer2AssociateDefaultViewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetView(AwsResourceExplorer2BatchGetViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2BatchGetViewOptions(), token);
    }

    public async Task<CommandResult> CreateIndex(AwsResourceExplorer2CreateIndexOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2CreateIndexOptions(), token);
    }

    public async Task<CommandResult> CreateView(AwsResourceExplorer2CreateViewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIndex(AwsResourceExplorer2DeleteIndexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteView(AwsResourceExplorer2DeleteViewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateDefaultView(AwsResourceExplorer2DisassociateDefaultViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2DisassociateDefaultViewOptions(), token);
    }

    public async Task<CommandResult> GetAccountLevelServiceConfiguration(AwsResourceExplorer2GetAccountLevelServiceConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2GetAccountLevelServiceConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetDefaultView(AwsResourceExplorer2GetDefaultViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2GetDefaultViewOptions(), token);
    }

    public async Task<CommandResult> GetIndex(AwsResourceExplorer2GetIndexOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2GetIndexOptions(), token);
    }

    public async Task<CommandResult> GetView(AwsResourceExplorer2GetViewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListIndexes(AwsResourceExplorer2ListIndexesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2ListIndexesOptions(), token);
    }

    public async Task<CommandResult> ListIndexesForMembers(AwsResourceExplorer2ListIndexesForMembersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSupportedResourceTypes(AwsResourceExplorer2ListSupportedResourceTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2ListSupportedResourceTypesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsResourceExplorer2ListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListViews(AwsResourceExplorer2ListViewsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourceExplorer2ListViewsOptions(), token);
    }

    public async Task<CommandResult> Search(AwsResourceExplorer2SearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsResourceExplorer2TagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsResourceExplorer2UntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateIndexType(AwsResourceExplorer2UpdateIndexTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateView(AwsResourceExplorer2UpdateViewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}