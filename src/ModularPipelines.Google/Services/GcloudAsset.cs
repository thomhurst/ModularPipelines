using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudAsset
{
    public GcloudAsset(
        GcloudAssetFeeds feeds,
        GcloudAssetOperations operations,
        GcloudAssetSavedQueries savedQueries,
        ICommand internalCommand
    )
    {
        Feeds = feeds;
        Operations = operations;
        SavedQueries = savedQueries;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudAssetFeeds Feeds { get; }

    public GcloudAssetOperations Operations { get; }

    public GcloudAssetSavedQueries SavedQueries { get; }

    public async Task<CommandResult> AnalyzeIamPolicy(GcloudAssetAnalyzeIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AnalyzeIamPolicyLongrunning(GcloudAssetAnalyzeIamPolicyLongrunningOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AnalyzeMove(GcloudAssetAnalyzeMoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(GcloudAssetExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEffectiveIamPolicy(GcloudAssetGetEffectiveIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetHistory(GcloudAssetGetHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudAssetListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Query(GcloudAssetQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchAllIamPolicies(GcloudAssetSearchAllIamPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAssetSearchAllIamPoliciesOptions(), token);
    }

    public async Task<CommandResult> SearchAllResources(GcloudAssetSearchAllResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAssetSearchAllResourcesOptions(), token);
    }
}