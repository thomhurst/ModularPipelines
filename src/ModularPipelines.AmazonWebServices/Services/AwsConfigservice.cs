using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> BatchGetAggregateResourceConfig(AwsConfigserviceBatchGetAggregateResourceConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchGetResourceConfig(AwsConfigserviceBatchGetResourceConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteAggregationAuthorization(AwsConfigserviceDeleteAggregationAuthorizationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConfigRule(AwsConfigserviceDeleteConfigRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConfigurationAggregator(AwsConfigserviceDeleteConfigurationAggregatorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConfigurationRecorder(AwsConfigserviceDeleteConfigurationRecorderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConformancePack(AwsConfigserviceDeleteConformancePackOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDeliveryChannel(AwsConfigserviceDeleteDeliveryChannelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteEvaluationResults(AwsConfigserviceDeleteEvaluationResultsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteOrganizationConfigRule(AwsConfigserviceDeleteOrganizationConfigRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteOrganizationConformancePack(AwsConfigserviceDeleteOrganizationConformancePackOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeletePendingAggregationRequest(AwsConfigserviceDeletePendingAggregationRequestOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteRemediationConfiguration(AwsConfigserviceDeleteRemediationConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteRemediationExceptions(AwsConfigserviceDeleteRemediationExceptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteResourceConfig(AwsConfigserviceDeleteResourceConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteRetentionConfiguration(AwsConfigserviceDeleteRetentionConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteStoredQuery(AwsConfigserviceDeleteStoredQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeliverConfigSnapshot(AwsConfigserviceDeliverConfigSnapshotOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAggregateComplianceByConfigRules(AwsConfigserviceDescribeAggregateComplianceByConfigRulesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAggregateComplianceByConformancePacks(AwsConfigserviceDescribeAggregateComplianceByConformancePacksOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAggregationAuthorizations(AwsConfigserviceDescribeAggregationAuthorizationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeAggregationAuthorizationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeComplianceByConfigRule(AwsConfigserviceDescribeComplianceByConfigRuleOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeComplianceByConfigRuleOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeComplianceByResource(AwsConfigserviceDescribeComplianceByResourceOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeComplianceByResourceOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConfigRuleEvaluationStatus(AwsConfigserviceDescribeConfigRuleEvaluationStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConfigRuleEvaluationStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConfigRules(AwsConfigserviceDescribeConfigRulesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConfigRulesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConfigurationAggregatorSourcesStatus(AwsConfigserviceDescribeConfigurationAggregatorSourcesStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConfigurationAggregators(AwsConfigserviceDescribeConfigurationAggregatorsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConfigurationAggregatorsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConfigurationRecorderStatus(AwsConfigserviceDescribeConfigurationRecorderStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConfigurationRecorderStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConfigurationRecorders(AwsConfigserviceDescribeConfigurationRecordersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConfigurationRecordersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConformancePackCompliance(AwsConfigserviceDescribeConformancePackComplianceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConformancePackStatus(AwsConfigserviceDescribeConformancePackStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConformancePackStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConformancePacks(AwsConfigserviceDescribeConformancePacksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeConformancePacksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDeliveryChannelStatus(AwsConfigserviceDescribeDeliveryChannelStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeDeliveryChannelStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeDeliveryChannels(AwsConfigserviceDescribeDeliveryChannelsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeDeliveryChannelsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrganizationConfigRuleStatuses(AwsConfigserviceDescribeOrganizationConfigRuleStatusesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeOrganizationConfigRuleStatusesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrganizationConfigRules(AwsConfigserviceDescribeOrganizationConfigRulesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeOrganizationConfigRulesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrganizationConformancePackStatuses(AwsConfigserviceDescribeOrganizationConformancePackStatusesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeOrganizationConformancePackStatusesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOrganizationConformancePacks(AwsConfigserviceDescribeOrganizationConformancePacksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeOrganizationConformancePacksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePendingAggregationRequests(AwsConfigserviceDescribePendingAggregationRequestsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribePendingAggregationRequestsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRemediationConfigurations(AwsConfigserviceDescribeRemediationConfigurationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRemediationExceptions(AwsConfigserviceDescribeRemediationExceptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRemediationExecutionStatus(AwsConfigserviceDescribeRemediationExecutionStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRetentionConfigurations(AwsConfigserviceDescribeRetentionConfigurationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceDescribeRetentionConfigurationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAggregateComplianceDetailsByConfigRule(AwsConfigserviceGetAggregateComplianceDetailsByConfigRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAggregateConfigRuleComplianceSummary(AwsConfigserviceGetAggregateConfigRuleComplianceSummaryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAggregateConformancePackComplianceSummary(AwsConfigserviceGetAggregateConformancePackComplianceSummaryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAggregateDiscoveredResourceCounts(AwsConfigserviceGetAggregateDiscoveredResourceCountsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAggregateResourceConfig(AwsConfigserviceGetAggregateResourceConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetComplianceDetailsByConfigRule(AwsConfigserviceGetComplianceDetailsByConfigRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetComplianceDetailsByResource(AwsConfigserviceGetComplianceDetailsByResourceOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetComplianceDetailsByResourceOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetComplianceSummaryByConfigRule(AwsConfigserviceGetComplianceSummaryByConfigRuleOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetComplianceSummaryByConfigRuleOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetComplianceSummaryByResourceType(AwsConfigserviceGetComplianceSummaryByResourceTypeOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetComplianceSummaryByResourceTypeOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConformancePackComplianceDetails(AwsConfigserviceGetConformancePackComplianceDetailsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConformancePackComplianceSummary(AwsConfigserviceGetConformancePackComplianceSummaryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetCustomRulePolicy(AwsConfigserviceGetCustomRulePolicyOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetCustomRulePolicyOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDiscoveredResourceCounts(AwsConfigserviceGetDiscoveredResourceCountsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetDiscoveredResourceCountsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetOrganizationConfigRuleDetailedStatus(AwsConfigserviceGetOrganizationConfigRuleDetailedStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetOrganizationConformancePackDetailedStatus(AwsConfigserviceGetOrganizationConformancePackDetailedStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetOrganizationCustomRulePolicy(AwsConfigserviceGetOrganizationCustomRulePolicyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetResourceConfigHistory(AwsConfigserviceGetResourceConfigHistoryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetResourceEvaluationSummary(AwsConfigserviceGetResourceEvaluationSummaryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetStatus(AwsConfigserviceGetStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceGetStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetStoredQuery(AwsConfigserviceGetStoredQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAggregateDiscoveredResources(AwsConfigserviceListAggregateDiscoveredResourcesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListConformancePackComplianceScores(AwsConfigserviceListConformancePackComplianceScoresOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceListConformancePackComplianceScoresOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDiscoveredResources(AwsConfigserviceListDiscoveredResourcesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListResourceEvaluations(AwsConfigserviceListResourceEvaluationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceListResourceEvaluationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListStoredQueries(AwsConfigserviceListStoredQueriesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceListStoredQueriesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsConfigserviceListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutAggregationAuthorization(AwsConfigservicePutAggregationAuthorizationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutConfigRule(AwsConfigservicePutConfigRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutConfigurationAggregator(AwsConfigservicePutConfigurationAggregatorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutConfigurationRecorder(AwsConfigservicePutConfigurationRecorderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutConformancePack(AwsConfigservicePutConformancePackOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutDeliveryChannel(AwsConfigservicePutDeliveryChannelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutEvaluations(AwsConfigservicePutEvaluationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutExternalEvaluation(AwsConfigservicePutExternalEvaluationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutOrganizationConfigRule(AwsConfigservicePutOrganizationConfigRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutOrganizationConformancePack(AwsConfigservicePutOrganizationConformancePackOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutRemediationConfigurations(AwsConfigservicePutRemediationConfigurationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutRemediationExceptions(AwsConfigservicePutRemediationExceptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutResourceConfig(AwsConfigservicePutResourceConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutRetentionConfiguration(AwsConfigservicePutRetentionConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutStoredQuery(AwsConfigservicePutStoredQueryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SelectAggregateResourceConfig(AwsConfigserviceSelectAggregateResourceConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SelectResourceConfig(AwsConfigserviceSelectResourceConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartConfigRulesEvaluation(AwsConfigserviceStartConfigRulesEvaluationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigserviceStartConfigRulesEvaluationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartConfigurationRecorder(AwsConfigserviceStartConfigurationRecorderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartRemediationExecution(AwsConfigserviceStartRemediationExecutionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartResourceEvaluation(AwsConfigserviceStartResourceEvaluationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopConfigurationRecorder(AwsConfigserviceStopConfigurationRecorderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> Subscribe(AwsConfigserviceSubscribeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsConfigserviceTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsConfigserviceUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}