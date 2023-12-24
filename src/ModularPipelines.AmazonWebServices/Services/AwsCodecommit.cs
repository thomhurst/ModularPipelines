using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCodecommit
{
    public AwsCodecommit(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateApprovalRuleTemplateWithRepository(AwsCodecommitAssociateApprovalRuleTemplateWithRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchAssociateApprovalRuleTemplateWithRepositories(AwsCodecommitBatchAssociateApprovalRuleTemplateWithRepositoriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDescribeMergeConflicts(AwsCodecommitBatchDescribeMergeConflictsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDisassociateApprovalRuleTemplateFromRepositories(AwsCodecommitBatchDisassociateApprovalRuleTemplateFromRepositoriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetCommits(AwsCodecommitBatchGetCommitsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetRepositories(AwsCodecommitBatchGetRepositoriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateApprovalRuleTemplate(AwsCodecommitCreateApprovalRuleTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBranch(AwsCodecommitCreateBranchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCommit(AwsCodecommitCreateCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePullRequest(AwsCodecommitCreatePullRequestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePullRequestApprovalRule(AwsCodecommitCreatePullRequestApprovalRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRepository(AwsCodecommitCreateRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUnreferencedMergeCommit(AwsCodecommitCreateUnreferencedMergeCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CredentialHelper(AwsCodecommitCredentialHelperOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodecommitCredentialHelperOptions(), token);
    }

    public async Task<CommandResult> DeleteApprovalRuleTemplate(AwsCodecommitDeleteApprovalRuleTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBranch(AwsCodecommitDeleteBranchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCommentContent(AwsCodecommitDeleteCommentContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFile(AwsCodecommitDeleteFileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePullRequestApprovalRule(AwsCodecommitDeletePullRequestApprovalRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRepository(AwsCodecommitDeleteRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMergeConflicts(AwsCodecommitDescribeMergeConflictsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePullRequestEvents(AwsCodecommitDescribePullRequestEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateApprovalRuleTemplateFromRepository(AwsCodecommitDisassociateApprovalRuleTemplateFromRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EvaluatePullRequestApprovalRules(AwsCodecommitEvaluatePullRequestApprovalRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetApprovalRuleTemplate(AwsCodecommitGetApprovalRuleTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBlob(AwsCodecommitGetBlobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBranch(AwsCodecommitGetBranchOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodecommitGetBranchOptions(), token);
    }

    public async Task<CommandResult> GetComment(AwsCodecommitGetCommentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCommentReactions(AwsCodecommitGetCommentReactionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCommentsForComparedCommit(AwsCodecommitGetCommentsForComparedCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCommentsForPullRequest(AwsCodecommitGetCommentsForPullRequestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCommit(AwsCodecommitGetCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDifferences(AwsCodecommitGetDifferencesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFile(AwsCodecommitGetFileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFolder(AwsCodecommitGetFolderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMergeCommit(AwsCodecommitGetMergeCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMergeConflicts(AwsCodecommitGetMergeConflictsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMergeOptions(AwsCodecommitGetMergeOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPullRequest(AwsCodecommitGetPullRequestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPullRequestApprovalStates(AwsCodecommitGetPullRequestApprovalStatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPullRequestOverrideState(AwsCodecommitGetPullRequestOverrideStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRepository(AwsCodecommitGetRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRepositoryTriggers(AwsCodecommitGetRepositoryTriggersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListApprovalRuleTemplates(AwsCodecommitListApprovalRuleTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodecommitListApprovalRuleTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListAssociatedApprovalRuleTemplatesForRepository(AwsCodecommitListAssociatedApprovalRuleTemplatesForRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBranches(AwsCodecommitListBranchesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFileCommitHistory(AwsCodecommitListFileCommitHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPullRequests(AwsCodecommitListPullRequestsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRepositories(AwsCodecommitListRepositoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodecommitListRepositoriesOptions(), token);
    }

    public async Task<CommandResult> ListRepositoriesForApprovalRuleTemplate(AwsCodecommitListRepositoriesForApprovalRuleTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsCodecommitListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MergeBranchesByFastForward(AwsCodecommitMergeBranchesByFastForwardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MergeBranchesBySquash(AwsCodecommitMergeBranchesBySquashOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MergeBranchesByThreeWay(AwsCodecommitMergeBranchesByThreeWayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MergePullRequestByFastForward(AwsCodecommitMergePullRequestByFastForwardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MergePullRequestBySquash(AwsCodecommitMergePullRequestBySquashOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MergePullRequestByThreeWay(AwsCodecommitMergePullRequestByThreeWayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> OverridePullRequestApprovalRules(AwsCodecommitOverridePullRequestApprovalRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PostCommentForComparedCommit(AwsCodecommitPostCommentForComparedCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PostCommentForPullRequest(AwsCodecommitPostCommentForPullRequestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PostCommentReply(AwsCodecommitPostCommentReplyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutCommentReaction(AwsCodecommitPutCommentReactionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutFile(AwsCodecommitPutFileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRepositoryTriggers(AwsCodecommitPutRepositoryTriggersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsCodecommitTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestRepositoryTriggers(AwsCodecommitTestRepositoryTriggersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsCodecommitUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApprovalRuleTemplateContent(AwsCodecommitUpdateApprovalRuleTemplateContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApprovalRuleTemplateDescription(AwsCodecommitUpdateApprovalRuleTemplateDescriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApprovalRuleTemplateName(AwsCodecommitUpdateApprovalRuleTemplateNameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateComment(AwsCodecommitUpdateCommentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDefaultBranch(AwsCodecommitUpdateDefaultBranchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePullRequestApprovalRuleContent(AwsCodecommitUpdatePullRequestApprovalRuleContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePullRequestApprovalState(AwsCodecommitUpdatePullRequestApprovalStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePullRequestDescription(AwsCodecommitUpdatePullRequestDescriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePullRequestStatus(AwsCodecommitUpdatePullRequestStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePullRequestTitle(AwsCodecommitUpdatePullRequestTitleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRepositoryDescription(AwsCodecommitUpdateRepositoryDescriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRepositoryEncryptionKey(AwsCodecommitUpdateRepositoryEncryptionKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRepositoryName(AwsCodecommitUpdateRepositoryNameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}