using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsMturk
{
    public AwsMturk(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptQualificationRequest(AwsMturkAcceptQualificationRequestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ApproveAssignment(AwsMturkApproveAssignmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateQualificationWithWorker(AwsMturkAssociateQualificationWithWorkerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAdditionalAssignmentsForHit(AwsMturkCreateAdditionalAssignmentsForHitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHit(AwsMturkCreateHitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHitType(AwsMturkCreateHitTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHitWithHitType(AwsMturkCreateHitWithHitTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateQualificationType(AwsMturkCreateQualificationTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorkerBlock(AwsMturkCreateWorkerBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteHit(AwsMturkDeleteHitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteQualificationType(AwsMturkDeleteQualificationTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWorkerBlock(AwsMturkDeleteWorkerBlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateQualificationFromWorker(AwsMturkDisassociateQualificationFromWorkerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccountBalance(AwsMturkGetAccountBalanceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMturkGetAccountBalanceOptions(), token);
    }

    public async Task<CommandResult> GetAssignment(AwsMturkGetAssignmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFileUploadUrl(AwsMturkGetFileUploadUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetHit(AwsMturkGetHitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetQualificationScore(AwsMturkGetQualificationScoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetQualificationType(AwsMturkGetQualificationTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssignmentsForHit(AwsMturkListAssignmentsForHitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBonusPayments(AwsMturkListBonusPaymentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMturkListBonusPaymentsOptions(), token);
    }

    public async Task<CommandResult> ListHits(AwsMturkListHitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMturkListHitsOptions(), token);
    }

    public async Task<CommandResult> ListHitsForQualificationType(AwsMturkListHitsForQualificationTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListQualificationRequests(AwsMturkListQualificationRequestsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMturkListQualificationRequestsOptions(), token);
    }

    public async Task<CommandResult> ListQualificationTypes(AwsMturkListQualificationTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMturkListQualificationTypesOptions(), token);
    }

    public async Task<CommandResult> ListReviewPolicyResultsForHit(AwsMturkListReviewPolicyResultsForHitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListReviewableHits(AwsMturkListReviewableHitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMturkListReviewableHitsOptions(), token);
    }

    public async Task<CommandResult> ListWorkerBlocks(AwsMturkListWorkerBlocksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMturkListWorkerBlocksOptions(), token);
    }

    public async Task<CommandResult> ListWorkersWithQualificationType(AwsMturkListWorkersWithQualificationTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NotifyWorkers(AwsMturkNotifyWorkersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectAssignment(AwsMturkRejectAssignmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectQualificationRequest(AwsMturkRejectQualificationRequestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendBonus(AwsMturkSendBonusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendTestEventNotification(AwsMturkSendTestEventNotificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateExpirationForHit(AwsMturkUpdateExpirationForHitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateHitReviewStatus(AwsMturkUpdateHitReviewStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateHitTypeOfHit(AwsMturkUpdateHitTypeOfHitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateNotificationSettings(AwsMturkUpdateNotificationSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateQualificationType(AwsMturkUpdateQualificationTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}