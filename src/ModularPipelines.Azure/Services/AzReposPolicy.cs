using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos")]
public class AzReposPolicy
{
    public AzReposPolicy(
        AzReposPolicyApproverCount approverCount,
        AzReposPolicyBuild build,
        AzReposPolicyCaseEnforcement caseEnforcement,
        AzReposPolicyCommentRequired commentRequired,
        AzReposPolicyFileSize fileSize,
        AzReposPolicyMergeStrategy mergeStrategy,
        AzReposPolicyRequiredReviewer requiredReviewer,
        AzReposPolicyWorkItemLinking workItemLinking,
        ICommand internalCommand
    )
    {
        ApproverCount = approverCount;
        Build = build;
        CaseEnforcement = caseEnforcement;
        CommentRequired = commentRequired;
        FileSize = fileSize;
        MergeStrategy = mergeStrategy;
        RequiredReviewer = requiredReviewer;
        WorkItemLinking = workItemLinking;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzReposPolicyApproverCount ApproverCount { get; }

    public AzReposPolicyBuild Build { get; }

    public AzReposPolicyCaseEnforcement CaseEnforcement { get; }

    public AzReposPolicyCommentRequired CommentRequired { get; }

    public AzReposPolicyFileSize FileSize { get; }

    public AzReposPolicyMergeStrategy MergeStrategy { get; }

    public AzReposPolicyRequiredReviewer RequiredReviewer { get; }

    public AzReposPolicyWorkItemLinking WorkItemLinking { get; }

    public async Task<CommandResult> Create(AzReposPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzReposPolicyDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzReposPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzReposPolicyShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzReposPolicyUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}