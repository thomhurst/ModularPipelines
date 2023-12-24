using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsIot
{
    public AwsIot(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptCertificateTransfer(AwsIotAcceptCertificateTransferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddThingToBillingGroup(AwsIotAddThingToBillingGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotAddThingToBillingGroupOptions(), token);
    }

    public async Task<CommandResult> AddThingToThingGroup(AwsIotAddThingToThingGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotAddThingToThingGroupOptions(), token);
    }

    public async Task<CommandResult> AssociateTargetsWithJob(AwsIotAssociateTargetsWithJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachPolicy(AwsIotAttachPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachSecurityProfile(AwsIotAttachSecurityProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachThingPrincipal(AwsIotAttachThingPrincipalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelAuditMitigationActionsTask(AwsIotCancelAuditMitigationActionsTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelAuditTask(AwsIotCancelAuditTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelCertificateTransfer(AwsIotCancelCertificateTransferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelDetectMitigationActionsTask(AwsIotCancelDetectMitigationActionsTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelJob(AwsIotCancelJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelJobExecution(AwsIotCancelJobExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ClearDefaultAuthorizer(AwsIotClearDefaultAuthorizerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotClearDefaultAuthorizerOptions(), token);
    }

    public async Task<CommandResult> ConfirmTopicRuleDestination(AwsIotConfirmTopicRuleDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAuditSuppression(AwsIotCreateAuditSuppressionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAuthorizer(AwsIotCreateAuthorizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBillingGroup(AwsIotCreateBillingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCertificateFromCsr(AwsIotCreateCertificateFromCsrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCertificateProvider(AwsIotCreateCertificateProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomMetric(AwsIotCreateCustomMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDimension(AwsIotCreateDimensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDomainConfiguration(AwsIotCreateDomainConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDynamicThingGroup(AwsIotCreateDynamicThingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFleetMetric(AwsIotCreateFleetMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateJob(AwsIotCreateJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateJobTemplate(AwsIotCreateJobTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateKeysAndCertificate(AwsIotCreateKeysAndCertificateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotCreateKeysAndCertificateOptions(), token);
    }

    public async Task<CommandResult> CreateMitigationAction(AwsIotCreateMitigationActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOtaUpdate(AwsIotCreateOtaUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePackage(AwsIotCreatePackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePackageVersion(AwsIotCreatePackageVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePolicy(AwsIotCreatePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePolicyVersion(AwsIotCreatePolicyVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProvisioningClaim(AwsIotCreateProvisioningClaimOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProvisioningTemplate(AwsIotCreateProvisioningTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProvisioningTemplateVersion(AwsIotCreateProvisioningTemplateVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRoleAlias(AwsIotCreateRoleAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateScheduledAudit(AwsIotCreateScheduledAuditOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSecurityProfile(AwsIotCreateSecurityProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStream(AwsIotCreateStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateThing(AwsIotCreateThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateThingGroup(AwsIotCreateThingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateThingType(AwsIotCreateThingTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTopicRule(AwsIotCreateTopicRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTopicRuleDestination(AwsIotCreateTopicRuleDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAccountAuditConfiguration(AwsIotDeleteAccountAuditConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotDeleteAccountAuditConfigurationOptions(), token);
    }

    public async Task<CommandResult> DeleteAuditSuppression(AwsIotDeleteAuditSuppressionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAuthorizer(AwsIotDeleteAuthorizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBillingGroup(AwsIotDeleteBillingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCaCertificate(AwsIotDeleteCaCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCertificate(AwsIotDeleteCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCertificateProvider(AwsIotDeleteCertificateProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomMetric(AwsIotDeleteCustomMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDimension(AwsIotDeleteDimensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDomainConfiguration(AwsIotDeleteDomainConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDynamicThingGroup(AwsIotDeleteDynamicThingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFleetMetric(AwsIotDeleteFleetMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteJob(AwsIotDeleteJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteJobExecution(AwsIotDeleteJobExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteJobTemplate(AwsIotDeleteJobTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMitigationAction(AwsIotDeleteMitigationActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOtaUpdate(AwsIotDeleteOtaUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePackage(AwsIotDeletePackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePackageVersion(AwsIotDeletePackageVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePolicy(AwsIotDeletePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePolicyVersion(AwsIotDeletePolicyVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProvisioningTemplate(AwsIotDeleteProvisioningTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProvisioningTemplateVersion(AwsIotDeleteProvisioningTemplateVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRegistrationCode(AwsIotDeleteRegistrationCodeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotDeleteRegistrationCodeOptions(), token);
    }

    public async Task<CommandResult> DeleteRoleAlias(AwsIotDeleteRoleAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteScheduledAudit(AwsIotDeleteScheduledAuditOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSecurityProfile(AwsIotDeleteSecurityProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStream(AwsIotDeleteStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteThing(AwsIotDeleteThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteThingGroup(AwsIotDeleteThingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteThingType(AwsIotDeleteThingTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTopicRule(AwsIotDeleteTopicRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTopicRuleDestination(AwsIotDeleteTopicRuleDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteV2LoggingLevel(AwsIotDeleteV2LoggingLevelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeprecateThingType(AwsIotDeprecateThingTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountAuditConfiguration(AwsIotDescribeAccountAuditConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotDescribeAccountAuditConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeAuditFinding(AwsIotDescribeAuditFindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAuditMitigationActionsTask(AwsIotDescribeAuditMitigationActionsTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAuditSuppression(AwsIotDescribeAuditSuppressionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAuditTask(AwsIotDescribeAuditTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAuthorizer(AwsIotDescribeAuthorizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeBillingGroup(AwsIotDescribeBillingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCaCertificate(AwsIotDescribeCaCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCertificate(AwsIotDescribeCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCertificateProvider(AwsIotDescribeCertificateProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCustomMetric(AwsIotDescribeCustomMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDefaultAuthorizer(AwsIotDescribeDefaultAuthorizerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotDescribeDefaultAuthorizerOptions(), token);
    }

    public async Task<CommandResult> DescribeDetectMitigationActionsTask(AwsIotDescribeDetectMitigationActionsTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDimension(AwsIotDescribeDimensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDomainConfiguration(AwsIotDescribeDomainConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEndpoint(AwsIotDescribeEndpointOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotDescribeEndpointOptions(), token);
    }

    public async Task<CommandResult> DescribeEventConfigurations(AwsIotDescribeEventConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotDescribeEventConfigurationsOptions(), token);
    }

    public async Task<CommandResult> DescribeFleetMetric(AwsIotDescribeFleetMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeIndex(AwsIotDescribeIndexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeJob(AwsIotDescribeJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeJobExecution(AwsIotDescribeJobExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeJobTemplate(AwsIotDescribeJobTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeManagedJobTemplate(AwsIotDescribeManagedJobTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMitigationAction(AwsIotDescribeMitigationActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeProvisioningTemplate(AwsIotDescribeProvisioningTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeProvisioningTemplateVersion(AwsIotDescribeProvisioningTemplateVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRoleAlias(AwsIotDescribeRoleAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeScheduledAudit(AwsIotDescribeScheduledAuditOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSecurityProfile(AwsIotDescribeSecurityProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStream(AwsIotDescribeStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeThing(AwsIotDescribeThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeThingGroup(AwsIotDescribeThingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeThingRegistrationTask(AwsIotDescribeThingRegistrationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeThingType(AwsIotDescribeThingTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachPolicy(AwsIotDetachPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachSecurityProfile(AwsIotDetachSecurityProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachThingPrincipal(AwsIotDetachThingPrincipalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableTopicRule(AwsIotDisableTopicRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableTopicRule(AwsIotEnableTopicRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBehaviorModelTrainingSummaries(AwsIotGetBehaviorModelTrainingSummariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotGetBehaviorModelTrainingSummariesOptions(), token);
    }

    public async Task<CommandResult> GetBucketsAggregation(AwsIotGetBucketsAggregationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCardinality(AwsIotGetCardinalityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEffectivePolicies(AwsIotGetEffectivePoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotGetEffectivePoliciesOptions(), token);
    }

    public async Task<CommandResult> GetIndexingConfiguration(AwsIotGetIndexingConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotGetIndexingConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetJobDocument(AwsIotGetJobDocumentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoggingOptions(AwsIotGetLoggingOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotGetLoggingOptionsOptions(), token);
    }

    public async Task<CommandResult> GetOtaUpdate(AwsIotGetOtaUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPackage(AwsIotGetPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPackageConfiguration(AwsIotGetPackageConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotGetPackageConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetPackageVersion(AwsIotGetPackageVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPercentiles(AwsIotGetPercentilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPolicy(AwsIotGetPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPolicyVersion(AwsIotGetPolicyVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRegistrationCode(AwsIotGetRegistrationCodeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotGetRegistrationCodeOptions(), token);
    }

    public async Task<CommandResult> GetStatistics(AwsIotGetStatisticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTopicRule(AwsIotGetTopicRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTopicRuleDestination(AwsIotGetTopicRuleDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetV2LoggingOptions(AwsIotGetV2LoggingOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotGetV2LoggingOptionsOptions(), token);
    }

    public async Task<CommandResult> ListActiveViolations(AwsIotListActiveViolationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListActiveViolationsOptions(), token);
    }

    public async Task<CommandResult> ListAttachedPolicies(AwsIotListAttachedPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAuditFindings(AwsIotListAuditFindingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListAuditFindingsOptions(), token);
    }

    public async Task<CommandResult> ListAuditMitigationActionsExecutions(AwsIotListAuditMitigationActionsExecutionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAuditMitigationActionsTasks(AwsIotListAuditMitigationActionsTasksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAuditSuppressions(AwsIotListAuditSuppressionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListAuditSuppressionsOptions(), token);
    }

    public async Task<CommandResult> ListAuditTasks(AwsIotListAuditTasksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAuthorizers(AwsIotListAuthorizersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListAuthorizersOptions(), token);
    }

    public async Task<CommandResult> ListBillingGroups(AwsIotListBillingGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListBillingGroupsOptions(), token);
    }

    public async Task<CommandResult> ListCaCertificates(AwsIotListCaCertificatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListCaCertificatesOptions(), token);
    }

    public async Task<CommandResult> ListCertificateProviders(AwsIotListCertificateProvidersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListCertificateProvidersOptions(), token);
    }

    public async Task<CommandResult> ListCertificates(AwsIotListCertificatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListCertificatesOptions(), token);
    }

    public async Task<CommandResult> ListCertificatesByCa(AwsIotListCertificatesByCaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomMetrics(AwsIotListCustomMetricsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListCustomMetricsOptions(), token);
    }

    public async Task<CommandResult> ListDetectMitigationActionsExecutions(AwsIotListDetectMitigationActionsExecutionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListDetectMitigationActionsExecutionsOptions(), token);
    }

    public async Task<CommandResult> ListDetectMitigationActionsTasks(AwsIotListDetectMitigationActionsTasksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDimensions(AwsIotListDimensionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListDimensionsOptions(), token);
    }

    public async Task<CommandResult> ListDomainConfigurations(AwsIotListDomainConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListDomainConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListFleetMetrics(AwsIotListFleetMetricsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListFleetMetricsOptions(), token);
    }

    public async Task<CommandResult> ListIndices(AwsIotListIndicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListIndicesOptions(), token);
    }

    public async Task<CommandResult> ListJobExecutionsForJob(AwsIotListJobExecutionsForJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListJobExecutionsForThing(AwsIotListJobExecutionsForThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListJobTemplates(AwsIotListJobTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListJobTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListJobs(AwsIotListJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListJobsOptions(), token);
    }

    public async Task<CommandResult> ListManagedJobTemplates(AwsIotListManagedJobTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListManagedJobTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListMetricValues(AwsIotListMetricValuesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMitigationActions(AwsIotListMitigationActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListMitigationActionsOptions(), token);
    }

    public async Task<CommandResult> ListOtaUpdates(AwsIotListOtaUpdatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListOtaUpdatesOptions(), token);
    }

    public async Task<CommandResult> ListOutgoingCertificates(AwsIotListOutgoingCertificatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListOutgoingCertificatesOptions(), token);
    }

    public async Task<CommandResult> ListPackageVersions(AwsIotListPackageVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPackages(AwsIotListPackagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListPackagesOptions(), token);
    }

    public async Task<CommandResult> ListPolicies(AwsIotListPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListPolicyVersions(AwsIotListPolicyVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPrincipalThings(AwsIotListPrincipalThingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListProvisioningTemplateVersions(AwsIotListProvisioningTemplateVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListProvisioningTemplates(AwsIotListProvisioningTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListProvisioningTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListRelatedResourcesForAuditFinding(AwsIotListRelatedResourcesForAuditFindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRoleAliases(AwsIotListRoleAliasesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListRoleAliasesOptions(), token);
    }

    public async Task<CommandResult> ListScheduledAudits(AwsIotListScheduledAuditsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListScheduledAuditsOptions(), token);
    }

    public async Task<CommandResult> ListSecurityProfiles(AwsIotListSecurityProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListSecurityProfilesOptions(), token);
    }

    public async Task<CommandResult> ListSecurityProfilesForTarget(AwsIotListSecurityProfilesForTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStreams(AwsIotListStreamsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListStreamsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsIotListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTargetsForPolicy(AwsIotListTargetsForPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTargetsForSecurityProfile(AwsIotListTargetsForSecurityProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListThingGroups(AwsIotListThingGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListThingGroupsOptions(), token);
    }

    public async Task<CommandResult> ListThingGroupsForThing(AwsIotListThingGroupsForThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListThingPrincipals(AwsIotListThingPrincipalsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListThingRegistrationTaskReports(AwsIotListThingRegistrationTaskReportsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListThingRegistrationTasks(AwsIotListThingRegistrationTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListThingRegistrationTasksOptions(), token);
    }

    public async Task<CommandResult> ListThingTypes(AwsIotListThingTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListThingTypesOptions(), token);
    }

    public async Task<CommandResult> ListThings(AwsIotListThingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListThingsOptions(), token);
    }

    public async Task<CommandResult> ListThingsInBillingGroup(AwsIotListThingsInBillingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListThingsInThingGroup(AwsIotListThingsInThingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTopicRuleDestinations(AwsIotListTopicRuleDestinationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListTopicRuleDestinationsOptions(), token);
    }

    public async Task<CommandResult> ListTopicRules(AwsIotListTopicRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListTopicRulesOptions(), token);
    }

    public async Task<CommandResult> ListV2LoggingLevels(AwsIotListV2LoggingLevelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotListV2LoggingLevelsOptions(), token);
    }

    public async Task<CommandResult> ListViolationEvents(AwsIotListViolationEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutVerificationStateOnViolation(AwsIotPutVerificationStateOnViolationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterCaCertificate(AwsIotRegisterCaCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterCertificate(AwsIotRegisterCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterCertificateWithoutCa(AwsIotRegisterCertificateWithoutCaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterThing(AwsIotRegisterThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectCertificateTransfer(AwsIotRejectCertificateTransferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveThingFromBillingGroup(AwsIotRemoveThingFromBillingGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotRemoveThingFromBillingGroupOptions(), token);
    }

    public async Task<CommandResult> RemoveThingFromThingGroup(AwsIotRemoveThingFromThingGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotRemoveThingFromThingGroupOptions(), token);
    }

    public async Task<CommandResult> ReplaceTopicRule(AwsIotReplaceTopicRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchIndex(AwsIotSearchIndexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetDefaultAuthorizer(AwsIotSetDefaultAuthorizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetDefaultPolicyVersion(AwsIotSetDefaultPolicyVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetLoggingOptions(AwsIotSetLoggingOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetV2LoggingLevel(AwsIotSetV2LoggingLevelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetV2LoggingOptions(AwsIotSetV2LoggingOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotSetV2LoggingOptionsOptions(), token);
    }

    public async Task<CommandResult> StartAuditMitigationActionsTask(AwsIotStartAuditMitigationActionsTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartDetectMitigationActionsTask(AwsIotStartDetectMitigationActionsTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartOnDemandAuditTask(AwsIotStartOnDemandAuditTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartThingRegistrationTask(AwsIotStartThingRegistrationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopThingRegistrationTask(AwsIotStopThingRegistrationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsIotTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestAuthorization(AwsIotTestAuthorizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestInvokeAuthorizer(AwsIotTestInvokeAuthorizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TransferCertificate(AwsIotTransferCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsIotUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAccountAuditConfiguration(AwsIotUpdateAccountAuditConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotUpdateAccountAuditConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateAuditSuppression(AwsIotUpdateAuditSuppressionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAuthorizer(AwsIotUpdateAuthorizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBillingGroup(AwsIotUpdateBillingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCaCertificate(AwsIotUpdateCaCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCertificate(AwsIotUpdateCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCertificateProvider(AwsIotUpdateCertificateProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCustomMetric(AwsIotUpdateCustomMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDimension(AwsIotUpdateDimensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDomainConfiguration(AwsIotUpdateDomainConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDynamicThingGroup(AwsIotUpdateDynamicThingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEventConfigurations(AwsIotUpdateEventConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotUpdateEventConfigurationsOptions(), token);
    }

    public async Task<CommandResult> UpdateFleetMetric(AwsIotUpdateFleetMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateIndexingConfiguration(AwsIotUpdateIndexingConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotUpdateIndexingConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateJob(AwsIotUpdateJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMitigationAction(AwsIotUpdateMitigationActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePackage(AwsIotUpdatePackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePackageConfiguration(AwsIotUpdatePackageConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotUpdatePackageConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdatePackageVersion(AwsIotUpdatePackageVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProvisioningTemplate(AwsIotUpdateProvisioningTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRoleAlias(AwsIotUpdateRoleAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateScheduledAudit(AwsIotUpdateScheduledAuditOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSecurityProfile(AwsIotUpdateSecurityProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStream(AwsIotUpdateStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateThing(AwsIotUpdateThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateThingGroup(AwsIotUpdateThingGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateThingGroupsForThing(AwsIotUpdateThingGroupsForThingOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotUpdateThingGroupsForThingOptions(), token);
    }

    public async Task<CommandResult> UpdateTopicRuleDestination(AwsIotUpdateTopicRuleDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ValidateSecurityProfileBehaviors(AwsIotValidateSecurityProfileBehaviorsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}