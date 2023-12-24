using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRoute53RecoveryReadiness
{
    public AwsRoute53RecoveryReadiness(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateCell(AwsRoute53RecoveryReadinessCreateCellOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCrossAccountAuthorization(AwsRoute53RecoveryReadinessCreateCrossAccountAuthorizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReadinessCheck(AwsRoute53RecoveryReadinessCreateReadinessCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRecoveryGroup(AwsRoute53RecoveryReadinessCreateRecoveryGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateResourceSet(AwsRoute53RecoveryReadinessCreateResourceSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCell(AwsRoute53RecoveryReadinessDeleteCellOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCrossAccountAuthorization(AwsRoute53RecoveryReadinessDeleteCrossAccountAuthorizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReadinessCheck(AwsRoute53RecoveryReadinessDeleteReadinessCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRecoveryGroup(AwsRoute53RecoveryReadinessDeleteRecoveryGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourceSet(AwsRoute53RecoveryReadinessDeleteResourceSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetArchitectureRecommendations(AwsRoute53RecoveryReadinessGetArchitectureRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCell(AwsRoute53RecoveryReadinessGetCellOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCellReadinessSummary(AwsRoute53RecoveryReadinessGetCellReadinessSummaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReadinessCheck(AwsRoute53RecoveryReadinessGetReadinessCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReadinessCheckResourceStatus(AwsRoute53RecoveryReadinessGetReadinessCheckResourceStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReadinessCheckStatus(AwsRoute53RecoveryReadinessGetReadinessCheckStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRecoveryGroup(AwsRoute53RecoveryReadinessGetRecoveryGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRecoveryGroupReadinessSummary(AwsRoute53RecoveryReadinessGetRecoveryGroupReadinessSummaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourceSet(AwsRoute53RecoveryReadinessGetResourceSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCells(AwsRoute53RecoveryReadinessListCellsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryReadinessListCellsOptions(), token);
    }

    public async Task<CommandResult> ListCrossAccountAuthorizations(AwsRoute53RecoveryReadinessListCrossAccountAuthorizationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryReadinessListCrossAccountAuthorizationsOptions(), token);
    }

    public async Task<CommandResult> ListReadinessChecks(AwsRoute53RecoveryReadinessListReadinessChecksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryReadinessListReadinessChecksOptions(), token);
    }

    public async Task<CommandResult> ListRecoveryGroups(AwsRoute53RecoveryReadinessListRecoveryGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryReadinessListRecoveryGroupsOptions(), token);
    }

    public async Task<CommandResult> ListResourceSets(AwsRoute53RecoveryReadinessListResourceSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryReadinessListResourceSetsOptions(), token);
    }

    public async Task<CommandResult> ListRules(AwsRoute53RecoveryReadinessListRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53RecoveryReadinessListRulesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResources(AwsRoute53RecoveryReadinessListTagsForResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsRoute53RecoveryReadinessTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsRoute53RecoveryReadinessUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCell(AwsRoute53RecoveryReadinessUpdateCellOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateReadinessCheck(AwsRoute53RecoveryReadinessUpdateReadinessCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRecoveryGroup(AwsRoute53RecoveryReadinessUpdateRecoveryGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResourceSet(AwsRoute53RecoveryReadinessUpdateResourceSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}