using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSagemaker
{
    public AwsSagemaker(
        AwsSagemakerWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsSagemakerWait Wait { get; }

    public async Task<CommandResult> AddAssociation(AwsSagemakerAddAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddTags(AwsSagemakerAddTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateTrialComponent(AwsSagemakerAssociateTrialComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDescribeModelPackage(AwsSagemakerBatchDescribeModelPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAction(AwsSagemakerCreateActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAlgorithm(AwsSagemakerCreateAlgorithmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateApp(AwsSagemakerCreateAppOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAppImageConfig(AwsSagemakerCreateAppImageConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateArtifact(AwsSagemakerCreateArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAutoMlJob(AwsSagemakerCreateAutoMlJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAutoMlJobV2(AwsSagemakerCreateAutoMlJobV2Options options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCluster(AwsSagemakerCreateClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCodeRepository(AwsSagemakerCreateCodeRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCompilationJob(AwsSagemakerCreateCompilationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateContext(AwsSagemakerCreateContextOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataQualityJobDefinition(AwsSagemakerCreateDataQualityJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDeviceFleet(AwsSagemakerCreateDeviceFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDomain(AwsSagemakerCreateDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEdgeDeploymentPlan(AwsSagemakerCreateEdgeDeploymentPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEdgeDeploymentStage(AwsSagemakerCreateEdgeDeploymentStageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEdgePackagingJob(AwsSagemakerCreateEdgePackagingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEndpoint(AwsSagemakerCreateEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEndpointConfig(AwsSagemakerCreateEndpointConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateExperiment(AwsSagemakerCreateExperimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFeatureGroup(AwsSagemakerCreateFeatureGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFlowDefinition(AwsSagemakerCreateFlowDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHub(AwsSagemakerCreateHubOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHumanTaskUi(AwsSagemakerCreateHumanTaskUiOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHyperParameterTuningJob(AwsSagemakerCreateHyperParameterTuningJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateImage(AwsSagemakerCreateImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateImageVersion(AwsSagemakerCreateImageVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInferenceComponent(AwsSagemakerCreateInferenceComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInferenceExperiment(AwsSagemakerCreateInferenceExperimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInferenceRecommendationsJob(AwsSagemakerCreateInferenceRecommendationsJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLabelingJob(AwsSagemakerCreateLabelingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateModel(AwsSagemakerCreateModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateModelBiasJobDefinition(AwsSagemakerCreateModelBiasJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateModelCard(AwsSagemakerCreateModelCardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateModelCardExportJob(AwsSagemakerCreateModelCardExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateModelExplainabilityJobDefinition(AwsSagemakerCreateModelExplainabilityJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateModelPackage(AwsSagemakerCreateModelPackageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerCreateModelPackageOptions(), token);
    }

    public async Task<CommandResult> CreateModelPackageGroup(AwsSagemakerCreateModelPackageGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateModelQualityJobDefinition(AwsSagemakerCreateModelQualityJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMonitoringSchedule(AwsSagemakerCreateMonitoringScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNotebookInstance(AwsSagemakerCreateNotebookInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNotebookInstanceLifecycleConfig(AwsSagemakerCreateNotebookInstanceLifecycleConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePipeline(AwsSagemakerCreatePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePresignedDomainUrl(AwsSagemakerCreatePresignedDomainUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePresignedNotebookInstanceUrl(AwsSagemakerCreatePresignedNotebookInstanceUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProcessingJob(AwsSagemakerCreateProcessingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProject(AwsSagemakerCreateProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSpace(AwsSagemakerCreateSpaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStudioLifecycleConfig(AwsSagemakerCreateStudioLifecycleConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrainingJob(AwsSagemakerCreateTrainingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTransformJob(AwsSagemakerCreateTransformJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrial(AwsSagemakerCreateTrialOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrialComponent(AwsSagemakerCreateTrialComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUserProfile(AwsSagemakerCreateUserProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorkforce(AwsSagemakerCreateWorkforceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorkteam(AwsSagemakerCreateWorkteamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAction(AwsSagemakerDeleteActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAlgorithm(AwsSagemakerDeleteAlgorithmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApp(AwsSagemakerDeleteAppOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppImageConfig(AwsSagemakerDeleteAppImageConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteArtifact(AwsSagemakerDeleteArtifactOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerDeleteArtifactOptions(), token);
    }

    public async Task<CommandResult> DeleteAssociation(AwsSagemakerDeleteAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCluster(AwsSagemakerDeleteClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCodeRepository(AwsSagemakerDeleteCodeRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCompilationJob(AwsSagemakerDeleteCompilationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteContext(AwsSagemakerDeleteContextOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataQualityJobDefinition(AwsSagemakerDeleteDataQualityJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDeviceFleet(AwsSagemakerDeleteDeviceFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDomain(AwsSagemakerDeleteDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEdgeDeploymentPlan(AwsSagemakerDeleteEdgeDeploymentPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEdgeDeploymentStage(AwsSagemakerDeleteEdgeDeploymentStageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEndpoint(AwsSagemakerDeleteEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEndpointConfig(AwsSagemakerDeleteEndpointConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteExperiment(AwsSagemakerDeleteExperimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFeatureGroup(AwsSagemakerDeleteFeatureGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFlowDefinition(AwsSagemakerDeleteFlowDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteHub(AwsSagemakerDeleteHubOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteHubContent(AwsSagemakerDeleteHubContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteHumanTaskUi(AwsSagemakerDeleteHumanTaskUiOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteImage(AwsSagemakerDeleteImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteImageVersion(AwsSagemakerDeleteImageVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInferenceComponent(AwsSagemakerDeleteInferenceComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInferenceExperiment(AwsSagemakerDeleteInferenceExperimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModel(AwsSagemakerDeleteModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModelBiasJobDefinition(AwsSagemakerDeleteModelBiasJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModelCard(AwsSagemakerDeleteModelCardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModelExplainabilityJobDefinition(AwsSagemakerDeleteModelExplainabilityJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModelPackage(AwsSagemakerDeleteModelPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModelPackageGroup(AwsSagemakerDeleteModelPackageGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModelPackageGroupPolicy(AwsSagemakerDeleteModelPackageGroupPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModelQualityJobDefinition(AwsSagemakerDeleteModelQualityJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMonitoringSchedule(AwsSagemakerDeleteMonitoringScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNotebookInstance(AwsSagemakerDeleteNotebookInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNotebookInstanceLifecycleConfig(AwsSagemakerDeleteNotebookInstanceLifecycleConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePipeline(AwsSagemakerDeletePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProject(AwsSagemakerDeleteProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSpace(AwsSagemakerDeleteSpaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStudioLifecycleConfig(AwsSagemakerDeleteStudioLifecycleConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTags(AwsSagemakerDeleteTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrial(AwsSagemakerDeleteTrialOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrialComponent(AwsSagemakerDeleteTrialComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUserProfile(AwsSagemakerDeleteUserProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWorkforce(AwsSagemakerDeleteWorkforceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWorkteam(AwsSagemakerDeleteWorkteamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterDevices(AwsSagemakerDeregisterDevicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAction(AwsSagemakerDescribeActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAlgorithm(AwsSagemakerDescribeAlgorithmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeApp(AwsSagemakerDescribeAppOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAppImageConfig(AwsSagemakerDescribeAppImageConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeArtifact(AwsSagemakerDescribeArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAutoMlJob(AwsSagemakerDescribeAutoMlJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAutoMlJobV2(AwsSagemakerDescribeAutoMlJobV2Options options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCluster(AwsSagemakerDescribeClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeClusterNode(AwsSagemakerDescribeClusterNodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCodeRepository(AwsSagemakerDescribeCodeRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCompilationJob(AwsSagemakerDescribeCompilationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeContext(AwsSagemakerDescribeContextOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDataQualityJobDefinition(AwsSagemakerDescribeDataQualityJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDevice(AwsSagemakerDescribeDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDeviceFleet(AwsSagemakerDescribeDeviceFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDomain(AwsSagemakerDescribeDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEdgeDeploymentPlan(AwsSagemakerDescribeEdgeDeploymentPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEdgePackagingJob(AwsSagemakerDescribeEdgePackagingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEndpoint(AwsSagemakerDescribeEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEndpointConfig(AwsSagemakerDescribeEndpointConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeExperiment(AwsSagemakerDescribeExperimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFeatureGroup(AwsSagemakerDescribeFeatureGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFeatureMetadata(AwsSagemakerDescribeFeatureMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFlowDefinition(AwsSagemakerDescribeFlowDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeHub(AwsSagemakerDescribeHubOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeHubContent(AwsSagemakerDescribeHubContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeHumanTaskUi(AwsSagemakerDescribeHumanTaskUiOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeHyperParameterTuningJob(AwsSagemakerDescribeHyperParameterTuningJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeImage(AwsSagemakerDescribeImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeImageVersion(AwsSagemakerDescribeImageVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInferenceComponent(AwsSagemakerDescribeInferenceComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInferenceExperiment(AwsSagemakerDescribeInferenceExperimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInferenceRecommendationsJob(AwsSagemakerDescribeInferenceRecommendationsJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLabelingJob(AwsSagemakerDescribeLabelingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLineageGroup(AwsSagemakerDescribeLineageGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeModel(AwsSagemakerDescribeModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeModelBiasJobDefinition(AwsSagemakerDescribeModelBiasJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeModelCard(AwsSagemakerDescribeModelCardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeModelCardExportJob(AwsSagemakerDescribeModelCardExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeModelExplainabilityJobDefinition(AwsSagemakerDescribeModelExplainabilityJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeModelPackage(AwsSagemakerDescribeModelPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeModelPackageGroup(AwsSagemakerDescribeModelPackageGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeModelQualityJobDefinition(AwsSagemakerDescribeModelQualityJobDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMonitoringSchedule(AwsSagemakerDescribeMonitoringScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeNotebookInstance(AwsSagemakerDescribeNotebookInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeNotebookInstanceLifecycleConfig(AwsSagemakerDescribeNotebookInstanceLifecycleConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePipeline(AwsSagemakerDescribePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePipelineDefinitionForExecution(AwsSagemakerDescribePipelineDefinitionForExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePipelineExecution(AwsSagemakerDescribePipelineExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeProcessingJob(AwsSagemakerDescribeProcessingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeProject(AwsSagemakerDescribeProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSpace(AwsSagemakerDescribeSpaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeStudioLifecycleConfig(AwsSagemakerDescribeStudioLifecycleConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSubscribedWorkteam(AwsSagemakerDescribeSubscribedWorkteamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrainingJob(AwsSagemakerDescribeTrainingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTransformJob(AwsSagemakerDescribeTransformJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrial(AwsSagemakerDescribeTrialOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrialComponent(AwsSagemakerDescribeTrialComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeUserProfile(AwsSagemakerDescribeUserProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorkforce(AwsSagemakerDescribeWorkforceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorkteam(AwsSagemakerDescribeWorkteamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableSagemakerServicecatalogPortfolio(AwsSagemakerDisableSagemakerServicecatalogPortfolioOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerDisableSagemakerServicecatalogPortfolioOptions(), token);
    }

    public async Task<CommandResult> DisassociateTrialComponent(AwsSagemakerDisassociateTrialComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableSagemakerServicecatalogPortfolio(AwsSagemakerEnableSagemakerServicecatalogPortfolioOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerEnableSagemakerServicecatalogPortfolioOptions(), token);
    }

    public async Task<CommandResult> GetDeviceFleetReport(AwsSagemakerGetDeviceFleetReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLineageGroupPolicy(AwsSagemakerGetLineageGroupPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetModelPackageGroupPolicy(AwsSagemakerGetModelPackageGroupPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSagemakerServicecatalogPortfolioStatus(AwsSagemakerGetSagemakerServicecatalogPortfolioStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerGetSagemakerServicecatalogPortfolioStatusOptions(), token);
    }

    public async Task<CommandResult> GetScalingConfigurationRecommendation(AwsSagemakerGetScalingConfigurationRecommendationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSearchSuggestions(AwsSagemakerGetSearchSuggestionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportHubContent(AwsSagemakerImportHubContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListActions(AwsSagemakerListActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListActionsOptions(), token);
    }

    public async Task<CommandResult> ListAlgorithms(AwsSagemakerListAlgorithmsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListAlgorithmsOptions(), token);
    }

    public async Task<CommandResult> ListAliases(AwsSagemakerListAliasesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppImageConfigs(AwsSagemakerListAppImageConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListAppImageConfigsOptions(), token);
    }

    public async Task<CommandResult> ListApps(AwsSagemakerListAppsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListAppsOptions(), token);
    }

    public async Task<CommandResult> ListArtifacts(AwsSagemakerListArtifactsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListArtifactsOptions(), token);
    }

    public async Task<CommandResult> ListAssociations(AwsSagemakerListAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListAutoMlJobs(AwsSagemakerListAutoMlJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListAutoMlJobsOptions(), token);
    }

    public async Task<CommandResult> ListCandidatesForAutoMlJob(AwsSagemakerListCandidatesForAutoMlJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListClusterNodes(AwsSagemakerListClusterNodesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListClusters(AwsSagemakerListClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListClustersOptions(), token);
    }

    public async Task<CommandResult> ListCodeRepositories(AwsSagemakerListCodeRepositoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListCodeRepositoriesOptions(), token);
    }

    public async Task<CommandResult> ListCompilationJobs(AwsSagemakerListCompilationJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListCompilationJobsOptions(), token);
    }

    public async Task<CommandResult> ListContexts(AwsSagemakerListContextsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListContextsOptions(), token);
    }

    public async Task<CommandResult> ListDataQualityJobDefinitions(AwsSagemakerListDataQualityJobDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListDataQualityJobDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListDeviceFleets(AwsSagemakerListDeviceFleetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListDeviceFleetsOptions(), token);
    }

    public async Task<CommandResult> ListDevices(AwsSagemakerListDevicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListDevicesOptions(), token);
    }

    public async Task<CommandResult> ListDomains(AwsSagemakerListDomainsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListDomainsOptions(), token);
    }

    public async Task<CommandResult> ListEdgeDeploymentPlans(AwsSagemakerListEdgeDeploymentPlansOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListEdgeDeploymentPlansOptions(), token);
    }

    public async Task<CommandResult> ListEdgePackagingJobs(AwsSagemakerListEdgePackagingJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListEdgePackagingJobsOptions(), token);
    }

    public async Task<CommandResult> ListEndpointConfigs(AwsSagemakerListEndpointConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListEndpointConfigsOptions(), token);
    }

    public async Task<CommandResult> ListEndpoints(AwsSagemakerListEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListEndpointsOptions(), token);
    }

    public async Task<CommandResult> ListExperiments(AwsSagemakerListExperimentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListExperimentsOptions(), token);
    }

    public async Task<CommandResult> ListFeatureGroups(AwsSagemakerListFeatureGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListFeatureGroupsOptions(), token);
    }

    public async Task<CommandResult> ListFlowDefinitions(AwsSagemakerListFlowDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListFlowDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListHubContentVersions(AwsSagemakerListHubContentVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListHubContents(AwsSagemakerListHubContentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListHubs(AwsSagemakerListHubsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListHubsOptions(), token);
    }

    public async Task<CommandResult> ListHumanTaskUis(AwsSagemakerListHumanTaskUisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListHumanTaskUisOptions(), token);
    }

    public async Task<CommandResult> ListHyperParameterTuningJobs(AwsSagemakerListHyperParameterTuningJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListHyperParameterTuningJobsOptions(), token);
    }

    public async Task<CommandResult> ListImageVersions(AwsSagemakerListImageVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListImages(AwsSagemakerListImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListImagesOptions(), token);
    }

    public async Task<CommandResult> ListInferenceComponents(AwsSagemakerListInferenceComponentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListInferenceComponentsOptions(), token);
    }

    public async Task<CommandResult> ListInferenceExperiments(AwsSagemakerListInferenceExperimentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListInferenceExperimentsOptions(), token);
    }

    public async Task<CommandResult> ListInferenceRecommendationsJobSteps(AwsSagemakerListInferenceRecommendationsJobStepsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInferenceRecommendationsJobs(AwsSagemakerListInferenceRecommendationsJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListInferenceRecommendationsJobsOptions(), token);
    }

    public async Task<CommandResult> ListLabelingJobs(AwsSagemakerListLabelingJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListLabelingJobsOptions(), token);
    }

    public async Task<CommandResult> ListLabelingJobsForWorkteam(AwsSagemakerListLabelingJobsForWorkteamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLineageGroups(AwsSagemakerListLineageGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListLineageGroupsOptions(), token);
    }

    public async Task<CommandResult> ListModelBiasJobDefinitions(AwsSagemakerListModelBiasJobDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListModelBiasJobDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListModelCardExportJobs(AwsSagemakerListModelCardExportJobsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListModelCardVersions(AwsSagemakerListModelCardVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListModelCards(AwsSagemakerListModelCardsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListModelCardsOptions(), token);
    }

    public async Task<CommandResult> ListModelExplainabilityJobDefinitions(AwsSagemakerListModelExplainabilityJobDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListModelExplainabilityJobDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListModelMetadata(AwsSagemakerListModelMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListModelMetadataOptions(), token);
    }

    public async Task<CommandResult> ListModelPackageGroups(AwsSagemakerListModelPackageGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListModelPackageGroupsOptions(), token);
    }

    public async Task<CommandResult> ListModelPackages(AwsSagemakerListModelPackagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListModelPackagesOptions(), token);
    }

    public async Task<CommandResult> ListModelQualityJobDefinitions(AwsSagemakerListModelQualityJobDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListModelQualityJobDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListModels(AwsSagemakerListModelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListModelsOptions(), token);
    }

    public async Task<CommandResult> ListMonitoringAlertHistory(AwsSagemakerListMonitoringAlertHistoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListMonitoringAlertHistoryOptions(), token);
    }

    public async Task<CommandResult> ListMonitoringAlerts(AwsSagemakerListMonitoringAlertsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMonitoringExecutions(AwsSagemakerListMonitoringExecutionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListMonitoringExecutionsOptions(), token);
    }

    public async Task<CommandResult> ListMonitoringSchedules(AwsSagemakerListMonitoringSchedulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListMonitoringSchedulesOptions(), token);
    }

    public async Task<CommandResult> ListNotebookInstanceLifecycleConfigs(AwsSagemakerListNotebookInstanceLifecycleConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListNotebookInstanceLifecycleConfigsOptions(), token);
    }

    public async Task<CommandResult> ListNotebookInstances(AwsSagemakerListNotebookInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListNotebookInstancesOptions(), token);
    }

    public async Task<CommandResult> ListPipelineExecutionSteps(AwsSagemakerListPipelineExecutionStepsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListPipelineExecutionStepsOptions(), token);
    }

    public async Task<CommandResult> ListPipelineExecutions(AwsSagemakerListPipelineExecutionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPipelineParametersForExecution(AwsSagemakerListPipelineParametersForExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPipelines(AwsSagemakerListPipelinesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListPipelinesOptions(), token);
    }

    public async Task<CommandResult> ListProcessingJobs(AwsSagemakerListProcessingJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListProcessingJobsOptions(), token);
    }

    public async Task<CommandResult> ListProjects(AwsSagemakerListProjectsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListProjectsOptions(), token);
    }

    public async Task<CommandResult> ListResourceCatalogs(AwsSagemakerListResourceCatalogsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListResourceCatalogsOptions(), token);
    }

    public async Task<CommandResult> ListSpaces(AwsSagemakerListSpacesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListSpacesOptions(), token);
    }

    public async Task<CommandResult> ListStageDevices(AwsSagemakerListStageDevicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStudioLifecycleConfigs(AwsSagemakerListStudioLifecycleConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListStudioLifecycleConfigsOptions(), token);
    }

    public async Task<CommandResult> ListSubscribedWorkteams(AwsSagemakerListSubscribedWorkteamsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListSubscribedWorkteamsOptions(), token);
    }

    public async Task<CommandResult> ListTags(AwsSagemakerListTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTrainingJobs(AwsSagemakerListTrainingJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListTrainingJobsOptions(), token);
    }

    public async Task<CommandResult> ListTrainingJobsForHyperParameterTuningJob(AwsSagemakerListTrainingJobsForHyperParameterTuningJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTransformJobs(AwsSagemakerListTransformJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListTransformJobsOptions(), token);
    }

    public async Task<CommandResult> ListTrialComponents(AwsSagemakerListTrialComponentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListTrialComponentsOptions(), token);
    }

    public async Task<CommandResult> ListTrials(AwsSagemakerListTrialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListTrialsOptions(), token);
    }

    public async Task<CommandResult> ListUserProfiles(AwsSagemakerListUserProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListUserProfilesOptions(), token);
    }

    public async Task<CommandResult> ListWorkforces(AwsSagemakerListWorkforcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListWorkforcesOptions(), token);
    }

    public async Task<CommandResult> ListWorkteams(AwsSagemakerListWorkteamsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerListWorkteamsOptions(), token);
    }

    public async Task<CommandResult> PutModelPackageGroupPolicy(AwsSagemakerPutModelPackageGroupPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> QueryLineage(AwsSagemakerQueryLineageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerQueryLineageOptions(), token);
    }

    public async Task<CommandResult> RegisterDevices(AwsSagemakerRegisterDevicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RenderUiTemplate(AwsSagemakerRenderUiTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RetryPipelineExecution(AwsSagemakerRetryPipelineExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Search(AwsSagemakerSearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendPipelineExecutionStepFailure(AwsSagemakerSendPipelineExecutionStepFailureOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendPipelineExecutionStepSuccess(AwsSagemakerSendPipelineExecutionStepSuccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartEdgeDeploymentStage(AwsSagemakerStartEdgeDeploymentStageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartInferenceExperiment(AwsSagemakerStartInferenceExperimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMonitoringSchedule(AwsSagemakerStartMonitoringScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartNotebookInstance(AwsSagemakerStartNotebookInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartPipelineExecution(AwsSagemakerStartPipelineExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopAutoMlJob(AwsSagemakerStopAutoMlJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopCompilationJob(AwsSagemakerStopCompilationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopEdgeDeploymentStage(AwsSagemakerStopEdgeDeploymentStageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopEdgePackagingJob(AwsSagemakerStopEdgePackagingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopHyperParameterTuningJob(AwsSagemakerStopHyperParameterTuningJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopInferenceExperiment(AwsSagemakerStopInferenceExperimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopInferenceRecommendationsJob(AwsSagemakerStopInferenceRecommendationsJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopLabelingJob(AwsSagemakerStopLabelingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopMonitoringSchedule(AwsSagemakerStopMonitoringScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopNotebookInstance(AwsSagemakerStopNotebookInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopPipelineExecution(AwsSagemakerStopPipelineExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopProcessingJob(AwsSagemakerStopProcessingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopTrainingJob(AwsSagemakerStopTrainingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopTransformJob(AwsSagemakerStopTransformJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAction(AwsSagemakerUpdateActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAppImageConfig(AwsSagemakerUpdateAppImageConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateArtifact(AwsSagemakerUpdateArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCluster(AwsSagemakerUpdateClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCodeRepository(AwsSagemakerUpdateCodeRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateContext(AwsSagemakerUpdateContextOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDeviceFleet(AwsSagemakerUpdateDeviceFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDevices(AwsSagemakerUpdateDevicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDomain(AwsSagemakerUpdateDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEndpoint(AwsSagemakerUpdateEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEndpointWeightsAndCapacities(AwsSagemakerUpdateEndpointWeightsAndCapacitiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateExperiment(AwsSagemakerUpdateExperimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFeatureGroup(AwsSagemakerUpdateFeatureGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFeatureMetadata(AwsSagemakerUpdateFeatureMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateHub(AwsSagemakerUpdateHubOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateImage(AwsSagemakerUpdateImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateImageVersion(AwsSagemakerUpdateImageVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInferenceComponent(AwsSagemakerUpdateInferenceComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInferenceComponentRuntimeConfig(AwsSagemakerUpdateInferenceComponentRuntimeConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInferenceExperiment(AwsSagemakerUpdateInferenceExperimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateModelCard(AwsSagemakerUpdateModelCardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateModelPackage(AwsSagemakerUpdateModelPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMonitoringAlert(AwsSagemakerUpdateMonitoringAlertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMonitoringSchedule(AwsSagemakerUpdateMonitoringScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateNotebookInstance(AwsSagemakerUpdateNotebookInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateNotebookInstanceLifecycleConfig(AwsSagemakerUpdateNotebookInstanceLifecycleConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePipeline(AwsSagemakerUpdatePipelineOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePipelineExecution(AwsSagemakerUpdatePipelineExecutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProject(AwsSagemakerUpdateProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSpace(AwsSagemakerUpdateSpaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTrainingJob(AwsSagemakerUpdateTrainingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTrial(AwsSagemakerUpdateTrialOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTrialComponent(AwsSagemakerUpdateTrialComponentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUserProfile(AwsSagemakerUpdateUserProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWorkforce(AwsSagemakerUpdateWorkforceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWorkteam(AwsSagemakerUpdateWorkteamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}