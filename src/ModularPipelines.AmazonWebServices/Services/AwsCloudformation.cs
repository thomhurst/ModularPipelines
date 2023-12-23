using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCloudformation
{
    public AwsCloudformation(
        AwsCloudformationWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsCloudformationWait Wait { get; }

    public async Task<CommandResult> ActivateOrganizationsAccess(AwsCloudformationActivateOrganizationsAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationActivateOrganizationsAccessOptions(), token);
    }

    public async Task<CommandResult> ActivateType(AwsCloudformationActivateTypeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationActivateTypeOptions(), token);
    }

    public async Task<CommandResult> BatchDescribeTypeConfigurations(AwsCloudformationBatchDescribeTypeConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelUpdateStack(AwsCloudformationCancelUpdateStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContinueUpdateRollback(AwsCloudformationContinueUpdateRollbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateChangeSet(AwsCloudformationCreateChangeSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStack(AwsCloudformationCreateStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStackInstances(AwsCloudformationCreateStackInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStackSet(AwsCloudformationCreateStackSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeactivateOrganizationsAccess(AwsCloudformationDeactivateOrganizationsAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationDeactivateOrganizationsAccessOptions(), token);
    }

    public async Task<CommandResult> DeactivateType(AwsCloudformationDeactivateTypeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationDeactivateTypeOptions(), token);
    }

    public async Task<CommandResult> DeleteChangeSet(AwsCloudformationDeleteChangeSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStack(AwsCloudformationDeleteStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStackInstances(AwsCloudformationDeleteStackInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStackSet(AwsCloudformationDeleteStackSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deploy(AwsCloudformationDeployOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterType(AwsCloudformationDeregisterTypeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationDeregisterTypeOptions(), token);
    }

    public async Task<CommandResult> DescribeAccountLimits(AwsCloudformationDescribeAccountLimitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationDescribeAccountLimitsOptions(), token);
    }

    public async Task<CommandResult> DescribeChangeSet(AwsCloudformationDescribeChangeSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeChangeSetHooks(AwsCloudformationDescribeChangeSetHooksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOrganizationsAccess(AwsCloudformationDescribeOrganizationsAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationDescribeOrganizationsAccessOptions(), token);
    }

    public async Task<CommandResult> DescribePublisher(AwsCloudformationDescribePublisherOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationDescribePublisherOptions(), token);
    }

    public async Task<CommandResult> DescribeStackDriftDetectionStatus(AwsCloudformationDescribeStackDriftDetectionStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStackEvents(AwsCloudformationDescribeStackEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationDescribeStackEventsOptions(), token);
    }

    public async Task<CommandResult> DescribeStackInstance(AwsCloudformationDescribeStackInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStackResource(AwsCloudformationDescribeStackResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStackResourceDrifts(AwsCloudformationDescribeStackResourceDriftsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStackResources(AwsCloudformationDescribeStackResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationDescribeStackResourcesOptions(), token);
    }

    public async Task<CommandResult> DescribeStackSet(AwsCloudformationDescribeStackSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStackSetOperation(AwsCloudformationDescribeStackSetOperationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStacks(AwsCloudformationDescribeStacksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationDescribeStacksOptions(), token);
    }

    public async Task<CommandResult> DescribeType(AwsCloudformationDescribeTypeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationDescribeTypeOptions(), token);
    }

    public async Task<CommandResult> DescribeTypeRegistration(AwsCloudformationDescribeTypeRegistrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectStackDrift(AwsCloudformationDetectStackDriftOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectStackResourceDrift(AwsCloudformationDetectStackResourceDriftOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectStackSetDrift(AwsCloudformationDetectStackSetDriftOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EstimateTemplateCost(AwsCloudformationEstimateTemplateCostOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationEstimateTemplateCostOptions(), token);
    }

    public async Task<CommandResult> ExecuteChangeSet(AwsCloudformationExecuteChangeSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStackPolicy(AwsCloudformationGetStackPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTemplate(AwsCloudformationGetTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationGetTemplateOptions(), token);
    }

    public async Task<CommandResult> GetTemplateSummary(AwsCloudformationGetTemplateSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationGetTemplateSummaryOptions(), token);
    }

    public async Task<CommandResult> ImportStacksToStackSet(AwsCloudformationImportStacksToStackSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChangeSets(AwsCloudformationListChangeSetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListExports(AwsCloudformationListExportsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationListExportsOptions(), token);
    }

    public async Task<CommandResult> ListImports(AwsCloudformationListImportsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStackInstanceResourceDrifts(AwsCloudformationListStackInstanceResourceDriftsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStackInstances(AwsCloudformationListStackInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStackResources(AwsCloudformationListStackResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStackSetOperationResults(AwsCloudformationListStackSetOperationResultsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStackSetOperations(AwsCloudformationListStackSetOperationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStackSets(AwsCloudformationListStackSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationListStackSetsOptions(), token);
    }

    public async Task<CommandResult> ListStacks(AwsCloudformationListStacksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationListStacksOptions(), token);
    }

    public async Task<CommandResult> ListTypeRegistrations(AwsCloudformationListTypeRegistrationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationListTypeRegistrationsOptions(), token);
    }

    public async Task<CommandResult> ListTypeVersions(AwsCloudformationListTypeVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationListTypeVersionsOptions(), token);
    }

    public async Task<CommandResult> ListTypes(AwsCloudformationListTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationListTypesOptions(), token);
    }

    public async Task<CommandResult> Package(AwsCloudformationPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PublishType(AwsCloudformationPublishTypeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationPublishTypeOptions(), token);
    }

    public async Task<CommandResult> RecordHandlerProgress(AwsCloudformationRecordHandlerProgressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterPublisher(AwsCloudformationRegisterPublisherOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationRegisterPublisherOptions(), token);
    }

    public async Task<CommandResult> RegisterType(AwsCloudformationRegisterTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RollbackStack(AwsCloudformationRollbackStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetStackPolicy(AwsCloudformationSetStackPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetTypeConfiguration(AwsCloudformationSetTypeConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetTypeDefaultVersion(AwsCloudformationSetTypeDefaultVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationSetTypeDefaultVersionOptions(), token);
    }

    public async Task<CommandResult> SignalResource(AwsCloudformationSignalResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopStackSetOperation(AwsCloudformationStopStackSetOperationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestType(AwsCloudformationTestTypeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationTestTypeOptions(), token);
    }

    public async Task<CommandResult> UpdateStack(AwsCloudformationUpdateStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStackInstances(AwsCloudformationUpdateStackInstancesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStackSet(AwsCloudformationUpdateStackSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTerminationProtection(AwsCloudformationUpdateTerminationProtectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ValidateTemplate(AwsCloudformationValidateTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationValidateTemplateOptions(), token);
    }
}