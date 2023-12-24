using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsConfigservice
{
    public AwsConfigservice(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchGetAggregateResourceConfig(AwsConfigserviceBatchGetAggregateResourceConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchGetResourceConfig(AwsConfigserviceBatchGetResourceConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAggregationAuthorization(AwsConfigserviceDeleteAggregationAuthorizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigRule(AwsConfigserviceDeleteConfigRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationAggregator(AwsConfigserviceDeleteConfigurationAggregatorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationRecorder(AwsConfigserviceDeleteConfigurationRecorderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConformancePack(AwsConfigserviceDeleteConformancePackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDeliveryChannel(AwsConfigserviceDeleteDeliveryChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEvaluationResults(AwsConfigserviceDeleteEvaluationResultsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOrganizationConfigRule(AwsConfigserviceDeleteOrganizationConfigRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOrganizationConformancePack(AwsConfigserviceDeleteOrganizationConformancePackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePendingAggregationRequest(AwsConfigserviceDeletePendingAggregationRequestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRemediationConfiguration(AwsConfigserviceDeleteRemediationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRemediationExceptions(AwsConfigserviceDeleteRemediationExceptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourceConfig(AwsConfigserviceDeleteResourceConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRetentionConfiguration(AwsConfigserviceDeleteRetentionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStoredQuery(AwsConfigserviceDeleteStoredQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeliverConfigSnapshot(AwsConfigserviceDeliverConfigSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAggregateComplianceByConfigRules(AwsConfigserviceDescribeAggregateComplianceByConfigRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAggregateComplianceByConformancePacks(AwsConfigserviceDescribeAggregateComplianceByConformancePacksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAggregationAuthorizations(AwsConfigserviceDescribeAggregationAuthorizationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeAggregationAuthorizationsOptions(), token);
    }

    public async Task<CommandResult> DescribeComplianceByConfigRule(AwsConfigserviceDescribeComplianceByConfigRuleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeComplianceByConfigRuleOptions(), token);
    }

    public async Task<CommandResult> DescribeComplianceByResource(AwsConfigserviceDescribeComplianceByResourceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeComplianceByResourceOptions(), token);
    }

    public async Task<CommandResult> DescribeConfigRuleEvaluationStatus(AwsConfigserviceDescribeConfigRuleEvaluationStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConfigRuleEvaluationStatusOptions(), token);
    }

    public async Task<CommandResult> DescribeConfigRules(AwsConfigserviceDescribeConfigRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConfigRulesOptions(), token);
    }

    public async Task<CommandResult> DescribeConfigurationAggregatorSourcesStatus(AwsConfigserviceDescribeConfigurationAggregatorSourcesStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConfigurationAggregators(AwsConfigserviceDescribeConfigurationAggregatorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConfigurationAggregatorsOptions(), token);
    }

    public async Task<CommandResult> DescribeConfigurationRecorderStatus(AwsConfigserviceDescribeConfigurationRecorderStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConfigurationRecorderStatusOptions(), token);
    }

    public async Task<CommandResult> DescribeConfigurationRecorders(AwsConfigserviceDescribeConfigurationRecordersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConfigurationRecordersOptions(), token);
    }

    public async Task<CommandResult> DescribeConformancePackCompliance(AwsConfigserviceDescribeConformancePackComplianceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConformancePackStatus(AwsConfigserviceDescribeConformancePackStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConformancePackStatusOptions(), token);
    }

    public async Task<CommandResult> DescribeConformancePacks(AwsConfigserviceDescribeConformancePacksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConformancePacksOptions(), token);
    }

    public async Task<CommandResult> DescribeDeliveryChannelStatus(AwsConfigserviceDescribeDeliveryChannelStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeDeliveryChannelStatusOptions(), token);
    }

    public async Task<CommandResult> DescribeDeliveryChannels(AwsConfigserviceDescribeDeliveryChannelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeDeliveryChannelsOptions(), token);
    }

    public async Task<CommandResult> DescribeOrganizationConfigRuleStatuses(AwsConfigserviceDescribeOrganizationConfigRuleStatusesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeOrganizationConfigRuleStatusesOptions(), token);
    }

    public async Task<CommandResult> DescribeOrganizationConfigRules(AwsConfigserviceDescribeOrganizationConfigRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeOrganizationConfigRulesOptions(), token);
    }

    public async Task<CommandResult> DescribeOrganizationConformancePackStatuses(AwsConfigserviceDescribeOrganizationConformancePackStatusesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeOrganizationConformancePackStatusesOptions(), token);
    }

    public async Task<CommandResult> DescribeOrganizationConformancePacks(AwsConfigserviceDescribeOrganizationConformancePacksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeOrganizationConformancePacksOptions(), token);
    }

    public async Task<CommandResult> DescribePendingAggregationRequests(AwsConfigserviceDescribePendingAggregationRequestsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribePendingAggregationRequestsOptions(), token);
    }

    public async Task<CommandResult> DescribeRemediationConfigurations(AwsConfigserviceDescribeRemediationConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRemediationExceptions(AwsConfigserviceDescribeRemediationExceptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRemediationExecutionStatus(AwsConfigserviceDescribeRemediationExecutionStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRetentionConfigurations(AwsConfigserviceDescribeRetentionConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeRetentionConfigurationsOptions(), token);
    }

    public async Task<CommandResult> GetAggregateComplianceDetailsByConfigRule(AwsConfigserviceGetAggregateComplianceDetailsByConfigRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAggregateConfigRuleComplianceSummary(AwsConfigserviceGetAggregateConfigRuleComplianceSummaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAggregateConformancePackComplianceSummary(AwsConfigserviceGetAggregateConformancePackComplianceSummaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAggregateDiscoveredResourceCounts(AwsConfigserviceGetAggregateDiscoveredResourceCountsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAggregateResourceConfig(AwsConfigserviceGetAggregateResourceConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetComplianceDetailsByConfigRule(AwsConfigserviceGetComplianceDetailsByConfigRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetComplianceDetailsByResource(AwsConfigserviceGetComplianceDetailsByResourceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetComplianceDetailsByResourceOptions(), token);
    }

    public async Task<CommandResult> GetComplianceSummaryByConfigRule(AwsConfigserviceGetComplianceSummaryByConfigRuleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetComplianceSummaryByConfigRuleOptions(), token);
    }

    public async Task<CommandResult> GetComplianceSummaryByResourceType(AwsConfigserviceGetComplianceSummaryByResourceTypeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetComplianceSummaryByResourceTypeOptions(), token);
    }

    public async Task<CommandResult> GetConformancePackComplianceDetails(AwsConfigserviceGetConformancePackComplianceDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConformancePackComplianceSummary(AwsConfigserviceGetConformancePackComplianceSummaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCustomRulePolicy(AwsConfigserviceGetCustomRulePolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetCustomRulePolicyOptions(), token);
    }

    public async Task<CommandResult> GetDiscoveredResourceCounts(AwsConfigserviceGetDiscoveredResourceCountsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetDiscoveredResourceCountsOptions(), token);
    }

    public async Task<CommandResult> GetOrganizationConfigRuleDetailedStatus(AwsConfigserviceGetOrganizationConfigRuleDetailedStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOrganizationConformancePackDetailedStatus(AwsConfigserviceGetOrganizationConformancePackDetailedStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOrganizationCustomRulePolicy(AwsConfigserviceGetOrganizationCustomRulePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourceConfigHistory(AwsConfigserviceGetResourceConfigHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourceEvaluationSummary(AwsConfigserviceGetResourceEvaluationSummaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStatus(AwsConfigserviceGetStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetStatusOptions(), token);
    }

    public async Task<CommandResult> GetStoredQuery(AwsConfigserviceGetStoredQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAggregateDiscoveredResources(AwsConfigserviceListAggregateDiscoveredResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListConformancePackComplianceScores(AwsConfigserviceListConformancePackComplianceScoresOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceListConformancePackComplianceScoresOptions(), token);
    }

    public async Task<CommandResult> ListDiscoveredResources(AwsConfigserviceListDiscoveredResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListResourceEvaluations(AwsConfigserviceListResourceEvaluationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceListResourceEvaluationsOptions(), token);
    }

    public async Task<CommandResult> ListStoredQueries(AwsConfigserviceListStoredQueriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceListStoredQueriesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsConfigserviceListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAggregationAuthorization(AwsConfigservicePutAggregationAuthorizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigRule(AwsConfigservicePutConfigRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationAggregator(AwsConfigservicePutConfigurationAggregatorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationRecorder(AwsConfigservicePutConfigurationRecorderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConformancePack(AwsConfigservicePutConformancePackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDeliveryChannel(AwsConfigservicePutDeliveryChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEvaluations(AwsConfigservicePutEvaluationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutExternalEvaluation(AwsConfigservicePutExternalEvaluationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutOrganizationConfigRule(AwsConfigservicePutOrganizationConfigRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutOrganizationConformancePack(AwsConfigservicePutOrganizationConformancePackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRemediationConfigurations(AwsConfigservicePutRemediationConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRemediationExceptions(AwsConfigservicePutRemediationExceptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourceConfig(AwsConfigservicePutResourceConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRetentionConfiguration(AwsConfigservicePutRetentionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutStoredQuery(AwsConfigservicePutStoredQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SelectAggregateResourceConfig(AwsConfigserviceSelectAggregateResourceConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SelectResourceConfig(AwsConfigserviceSelectResourceConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartConfigRulesEvaluation(AwsConfigserviceStartConfigRulesEvaluationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceStartConfigRulesEvaluationOptions(), token);
    }

    public async Task<CommandResult> StartConfigurationRecorder(AwsConfigserviceStartConfigurationRecorderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartRemediationExecution(AwsConfigserviceStartRemediationExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartResourceEvaluation(AwsConfigserviceStartResourceEvaluationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopConfigurationRecorder(AwsConfigserviceStopConfigurationRecorderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Subscribe(AwsConfigserviceSubscribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsConfigserviceTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsConfigserviceUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}