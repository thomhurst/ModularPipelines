using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCleanrooms
{
    public AwsCleanrooms(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchGetCollaborationAnalysisTemplate(AwsCleanroomsBatchGetCollaborationAnalysisTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetSchema(AwsCleanroomsBatchGetSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAnalysisTemplate(AwsCleanroomsCreateAnalysisTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCollaboration(AwsCleanroomsCreateCollaborationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfiguredAudienceModelAssociation(AwsCleanroomsCreateConfiguredAudienceModelAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfiguredTable(AwsCleanroomsCreateConfiguredTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfiguredTableAnalysisRule(AwsCleanroomsCreateConfiguredTableAnalysisRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfiguredTableAssociation(AwsCleanroomsCreateConfiguredTableAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMembership(AwsCleanroomsCreateMembershipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePrivacyBudgetTemplate(AwsCleanroomsCreatePrivacyBudgetTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAnalysisTemplate(AwsCleanroomsDeleteAnalysisTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCollaboration(AwsCleanroomsDeleteCollaborationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfiguredAudienceModelAssociation(AwsCleanroomsDeleteConfiguredAudienceModelAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfiguredTable(AwsCleanroomsDeleteConfiguredTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfiguredTableAnalysisRule(AwsCleanroomsDeleteConfiguredTableAnalysisRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfiguredTableAssociation(AwsCleanroomsDeleteConfiguredTableAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMember(AwsCleanroomsDeleteMemberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMembership(AwsCleanroomsDeleteMembershipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePrivacyBudgetTemplate(AwsCleanroomsDeletePrivacyBudgetTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAnalysisTemplate(AwsCleanroomsGetAnalysisTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCollaboration(AwsCleanroomsGetCollaborationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCollaborationAnalysisTemplate(AwsCleanroomsGetCollaborationAnalysisTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCollaborationConfiguredAudienceModelAssociation(AwsCleanroomsGetCollaborationConfiguredAudienceModelAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCollaborationPrivacyBudgetTemplate(AwsCleanroomsGetCollaborationPrivacyBudgetTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfiguredAudienceModelAssociation(AwsCleanroomsGetConfiguredAudienceModelAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfiguredTable(AwsCleanroomsGetConfiguredTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfiguredTableAnalysisRule(AwsCleanroomsGetConfiguredTableAnalysisRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfiguredTableAssociation(AwsCleanroomsGetConfiguredTableAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMembership(AwsCleanroomsGetMembershipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPrivacyBudgetTemplate(AwsCleanroomsGetPrivacyBudgetTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetProtectedQuery(AwsCleanroomsGetProtectedQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSchema(AwsCleanroomsGetSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSchemaAnalysisRule(AwsCleanroomsGetSchemaAnalysisRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAnalysisTemplates(AwsCleanroomsListAnalysisTemplatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCollaborationAnalysisTemplates(AwsCleanroomsListCollaborationAnalysisTemplatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCollaborationConfiguredAudienceModelAssociations(AwsCleanroomsListCollaborationConfiguredAudienceModelAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCollaborationPrivacyBudgetTemplates(AwsCleanroomsListCollaborationPrivacyBudgetTemplatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCollaborationPrivacyBudgets(AwsCleanroomsListCollaborationPrivacyBudgetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCollaborations(AwsCleanroomsListCollaborationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCleanroomsListCollaborationsOptions(), token);
    }

    public async Task<CommandResult> ListConfiguredAudienceModelAssociations(AwsCleanroomsListConfiguredAudienceModelAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListConfiguredTableAssociations(AwsCleanroomsListConfiguredTableAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListConfiguredTables(AwsCleanroomsListConfiguredTablesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCleanroomsListConfiguredTablesOptions(), token);
    }

    public async Task<CommandResult> ListMembers(AwsCleanroomsListMembersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMemberships(AwsCleanroomsListMembershipsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCleanroomsListMembershipsOptions(), token);
    }

    public async Task<CommandResult> ListPrivacyBudgetTemplates(AwsCleanroomsListPrivacyBudgetTemplatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPrivacyBudgets(AwsCleanroomsListPrivacyBudgetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListProtectedQueries(AwsCleanroomsListProtectedQueriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSchemas(AwsCleanroomsListSchemasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsCleanroomsListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PreviewPrivacyImpact(AwsCleanroomsPreviewPrivacyImpactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartProtectedQuery(AwsCleanroomsStartProtectedQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsCleanroomsTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsCleanroomsUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAnalysisTemplate(AwsCleanroomsUpdateAnalysisTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCollaboration(AwsCleanroomsUpdateCollaborationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfiguredAudienceModelAssociation(AwsCleanroomsUpdateConfiguredAudienceModelAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfiguredTable(AwsCleanroomsUpdateConfiguredTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfiguredTableAnalysisRule(AwsCleanroomsUpdateConfiguredTableAnalysisRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfiguredTableAssociation(AwsCleanroomsUpdateConfiguredTableAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMembership(AwsCleanroomsUpdateMembershipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePrivacyBudgetTemplate(AwsCleanroomsUpdatePrivacyBudgetTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProtectedQuery(AwsCleanroomsUpdateProtectedQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}