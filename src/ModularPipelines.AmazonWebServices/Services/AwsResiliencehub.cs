using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsResiliencehub
{
    public AwsResiliencehub(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddDraftAppVersionResourceMappings(AwsResiliencehubAddDraftAppVersionResourceMappingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchUpdateRecommendationStatus(AwsResiliencehubBatchUpdateRecommendationStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateApp(AwsResiliencehubCreateAppOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAppVersionAppComponent(AwsResiliencehubCreateAppVersionAppComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAppVersionResource(AwsResiliencehubCreateAppVersionResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRecommendationTemplate(AwsResiliencehubCreateRecommendationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateResiliencyPolicy(AwsResiliencehubCreateResiliencyPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApp(AwsResiliencehubDeleteAppOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppAssessment(AwsResiliencehubDeleteAppAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppInputSource(AwsResiliencehubDeleteAppInputSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppVersionAppComponent(AwsResiliencehubDeleteAppVersionAppComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppVersionResource(AwsResiliencehubDeleteAppVersionResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRecommendationTemplate(AwsResiliencehubDeleteRecommendationTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResiliencyPolicy(AwsResiliencehubDeleteResiliencyPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeApp(AwsResiliencehubDescribeAppOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppAssessment(AwsResiliencehubDescribeAppAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppVersion(AwsResiliencehubDescribeAppVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppVersionAppComponent(AwsResiliencehubDescribeAppVersionAppComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppVersionResource(AwsResiliencehubDescribeAppVersionResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppVersionResourcesResolutionStatus(AwsResiliencehubDescribeAppVersionResourcesResolutionStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppVersionTemplate(AwsResiliencehubDescribeAppVersionTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDraftAppVersionResourcesImportStatus(AwsResiliencehubDescribeDraftAppVersionResourcesImportStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeResiliencyPolicy(AwsResiliencehubDescribeResiliencyPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportResourcesToDraftAppVersion(AwsResiliencehubImportResourcesToDraftAppVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAlarmRecommendations(AwsResiliencehubListAlarmRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppAssessmentComplianceDrifts(AwsResiliencehubListAppAssessmentComplianceDriftsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppAssessments(AwsResiliencehubListAppAssessmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResiliencehubListAppAssessmentsOptions(), token);
    }

    public async Task<CommandResult> ListAppComponentCompliances(AwsResiliencehubListAppComponentCompliancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppComponentRecommendations(AwsResiliencehubListAppComponentRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppInputSources(AwsResiliencehubListAppInputSourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppVersionAppComponents(AwsResiliencehubListAppVersionAppComponentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppVersionResourceMappings(AwsResiliencehubListAppVersionResourceMappingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppVersionResources(AwsResiliencehubListAppVersionResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppVersions(AwsResiliencehubListAppVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListApps(AwsResiliencehubListAppsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResiliencehubListAppsOptions(), token);
    }

    public async Task<CommandResult> ListRecommendationTemplates(AwsResiliencehubListRecommendationTemplatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListResiliencyPolicies(AwsResiliencehubListResiliencyPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResiliencehubListResiliencyPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListSopRecommendations(AwsResiliencehubListSopRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSuggestedResiliencyPolicies(AwsResiliencehubListSuggestedResiliencyPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResiliencehubListSuggestedResiliencyPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsResiliencehubListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTestRecommendations(AwsResiliencehubListTestRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUnsupportedAppVersionResources(AwsResiliencehubListUnsupportedAppVersionResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PublishAppVersion(AwsResiliencehubPublishAppVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDraftAppVersionTemplate(AwsResiliencehubPutDraftAppVersionTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveDraftAppVersionResourceMappings(AwsResiliencehubRemoveDraftAppVersionResourceMappingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResolveAppVersionResources(AwsResiliencehubResolveAppVersionResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartAppAssessment(AwsResiliencehubStartAppAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsResiliencehubTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsResiliencehubUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApp(AwsResiliencehubUpdateAppOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAppVersion(AwsResiliencehubUpdateAppVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAppVersionAppComponent(AwsResiliencehubUpdateAppVersionAppComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAppVersionResource(AwsResiliencehubUpdateAppVersionResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResiliencyPolicy(AwsResiliencehubUpdateResiliencyPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}