using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsImagebuilder
{
    public AwsImagebuilder(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CancelImageCreation(AwsImagebuilderCancelImageCreationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelLifecycleExecution(AwsImagebuilderCancelLifecycleExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateComponent(AwsImagebuilderCreateComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateContainerRecipe(AwsImagebuilderCreateContainerRecipeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDistributionConfiguration(AwsImagebuilderCreateDistributionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateImage(AwsImagebuilderCreateImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateImagePipeline(AwsImagebuilderCreateImagePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateImageRecipe(AwsImagebuilderCreateImageRecipeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInfrastructureConfiguration(AwsImagebuilderCreateInfrastructureConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLifecyclePolicy(AwsImagebuilderCreateLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorkflow(AwsImagebuilderCreateWorkflowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteComponent(AwsImagebuilderDeleteComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteContainerRecipe(AwsImagebuilderDeleteContainerRecipeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDistributionConfiguration(AwsImagebuilderDeleteDistributionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteImage(AwsImagebuilderDeleteImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteImagePipeline(AwsImagebuilderDeleteImagePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteImageRecipe(AwsImagebuilderDeleteImageRecipeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInfrastructureConfiguration(AwsImagebuilderDeleteInfrastructureConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLifecyclePolicy(AwsImagebuilderDeleteLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWorkflow(AwsImagebuilderDeleteWorkflowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetComponent(AwsImagebuilderGetComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetComponentPolicy(AwsImagebuilderGetComponentPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContainerRecipe(AwsImagebuilderGetContainerRecipeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContainerRecipePolicy(AwsImagebuilderGetContainerRecipePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDistributionConfiguration(AwsImagebuilderGetDistributionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetImage(AwsImagebuilderGetImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetImagePipeline(AwsImagebuilderGetImagePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetImagePolicy(AwsImagebuilderGetImagePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetImageRecipe(AwsImagebuilderGetImageRecipeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetImageRecipePolicy(AwsImagebuilderGetImageRecipePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInfrastructureConfiguration(AwsImagebuilderGetInfrastructureConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLifecycleExecution(AwsImagebuilderGetLifecycleExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLifecyclePolicy(AwsImagebuilderGetLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorkflow(AwsImagebuilderGetWorkflowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorkflowExecution(AwsImagebuilderGetWorkflowExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWorkflowStepExecution(AwsImagebuilderGetWorkflowStepExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportComponent(AwsImagebuilderImportComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportVmImage(AwsImagebuilderImportVmImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListComponentBuildVersions(AwsImagebuilderListComponentBuildVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListComponents(AwsImagebuilderListComponentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListComponentsOptions(), token);
    }

    public async Task<CommandResult> ListContainerRecipes(AwsImagebuilderListContainerRecipesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListContainerRecipesOptions(), token);
    }

    public async Task<CommandResult> ListDistributionConfigurations(AwsImagebuilderListDistributionConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListDistributionConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListImageBuildVersions(AwsImagebuilderListImageBuildVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListImagePackages(AwsImagebuilderListImagePackagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListImagePipelineImages(AwsImagebuilderListImagePipelineImagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListImagePipelines(AwsImagebuilderListImagePipelinesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListImagePipelinesOptions(), token);
    }

    public async Task<CommandResult> ListImageRecipes(AwsImagebuilderListImageRecipesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListImageRecipesOptions(), token);
    }

    public async Task<CommandResult> ListImageScanFindingAggregations(AwsImagebuilderListImageScanFindingAggregationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListImageScanFindingAggregationsOptions(), token);
    }

    public async Task<CommandResult> ListImageScanFindings(AwsImagebuilderListImageScanFindingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListImageScanFindingsOptions(), token);
    }

    public async Task<CommandResult> ListImages(AwsImagebuilderListImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListImagesOptions(), token);
    }

    public async Task<CommandResult> ListInfrastructureConfigurations(AwsImagebuilderListInfrastructureConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListInfrastructureConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListLifecycleExecutionResources(AwsImagebuilderListLifecycleExecutionResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLifecycleExecutions(AwsImagebuilderListLifecycleExecutionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLifecyclePolicies(AwsImagebuilderListLifecyclePoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListLifecyclePoliciesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsImagebuilderListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListWaitingWorkflowSteps(AwsImagebuilderListWaitingWorkflowStepsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListWaitingWorkflowStepsOptions(), token);
    }

    public async Task<CommandResult> ListWorkflowBuildVersions(AwsImagebuilderListWorkflowBuildVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListWorkflowExecutions(AwsImagebuilderListWorkflowExecutionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListWorkflowStepExecutions(AwsImagebuilderListWorkflowStepExecutionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListWorkflows(AwsImagebuilderListWorkflowsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsImagebuilderListWorkflowsOptions(), token);
    }

    public async Task<CommandResult> PutComponentPolicy(AwsImagebuilderPutComponentPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutContainerRecipePolicy(AwsImagebuilderPutContainerRecipePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutImagePolicy(AwsImagebuilderPutImagePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutImageRecipePolicy(AwsImagebuilderPutImageRecipePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendWorkflowStepAction(AwsImagebuilderSendWorkflowStepActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartImagePipelineExecution(AwsImagebuilderStartImagePipelineExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartResourceStateUpdate(AwsImagebuilderStartResourceStateUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsImagebuilderTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsImagebuilderUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDistributionConfiguration(AwsImagebuilderUpdateDistributionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateImagePipeline(AwsImagebuilderUpdateImagePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInfrastructureConfiguration(AwsImagebuilderUpdateInfrastructureConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLifecyclePolicy(AwsImagebuilderUpdateLifecyclePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}