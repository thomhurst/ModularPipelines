using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRekognition
{
    public AwsRekognition(
        AwsRekognitionWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsRekognitionWait Wait { get; }

    public async Task<CommandResult> AssociateFaces(AwsRekognitionAssociateFacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CompareFaces(AwsRekognitionCompareFacesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionCompareFacesOptions(), token);
    }

    public async Task<CommandResult> CopyProjectVersion(AwsRekognitionCopyProjectVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCollection(AwsRekognitionCreateCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataset(AwsRekognitionCreateDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFaceLivenessSession(AwsRekognitionCreateFaceLivenessSessionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionCreateFaceLivenessSessionOptions(), token);
    }

    public async Task<CommandResult> CreateProject(AwsRekognitionCreateProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProjectVersion(AwsRekognitionCreateProjectVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStreamProcessor(AwsRekognitionCreateStreamProcessorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUser(AwsRekognitionCreateUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCollection(AwsRekognitionDeleteCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataset(AwsRekognitionDeleteDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFaces(AwsRekognitionDeleteFacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProject(AwsRekognitionDeleteProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProjectPolicy(AwsRekognitionDeleteProjectPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProjectVersion(AwsRekognitionDeleteProjectVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteStreamProcessor(AwsRekognitionDeleteStreamProcessorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUser(AwsRekognitionDeleteUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCollection(AwsRekognitionDescribeCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDataset(AwsRekognitionDescribeDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeProjectVersions(AwsRekognitionDescribeProjectVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeProjects(AwsRekognitionDescribeProjectsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionDescribeProjectsOptions(), token);
    }

    public async Task<CommandResult> DescribeStreamProcessor(AwsRekognitionDescribeStreamProcessorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectCustomLabels(AwsRekognitionDetectCustomLabelsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectFaces(AwsRekognitionDetectFacesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionDetectFacesOptions(), token);
    }

    public async Task<CommandResult> DetectLabels(AwsRekognitionDetectLabelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionDetectLabelsOptions(), token);
    }

    public async Task<CommandResult> DetectModerationLabels(AwsRekognitionDetectModerationLabelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionDetectModerationLabelsOptions(), token);
    }

    public async Task<CommandResult> DetectProtectiveEquipment(AwsRekognitionDetectProtectiveEquipmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionDetectProtectiveEquipmentOptions(), token);
    }

    public async Task<CommandResult> DetectText(AwsRekognitionDetectTextOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionDetectTextOptions(), token);
    }

    public async Task<CommandResult> DisassociateFaces(AwsRekognitionDisassociateFacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DistributeDatasetEntries(AwsRekognitionDistributeDatasetEntriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCelebrityInfo(AwsRekognitionGetCelebrityInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCelebrityRecognition(AwsRekognitionGetCelebrityRecognitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContentModeration(AwsRekognitionGetContentModerationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFaceDetection(AwsRekognitionGetFaceDetectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFaceLivenessSessionResults(AwsRekognitionGetFaceLivenessSessionResultsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFaceSearch(AwsRekognitionGetFaceSearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLabelDetection(AwsRekognitionGetLabelDetectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMediaAnalysisJob(AwsRekognitionGetMediaAnalysisJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPersonTracking(AwsRekognitionGetPersonTrackingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSegmentDetection(AwsRekognitionGetSegmentDetectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTextDetection(AwsRekognitionGetTextDetectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> IndexFaces(AwsRekognitionIndexFacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCollections(AwsRekognitionListCollectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionListCollectionsOptions(), token);
    }

    public async Task<CommandResult> ListDatasetEntries(AwsRekognitionListDatasetEntriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDatasetLabels(AwsRekognitionListDatasetLabelsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFaces(AwsRekognitionListFacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMediaAnalysisJobs(AwsRekognitionListMediaAnalysisJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionListMediaAnalysisJobsOptions(), token);
    }

    public async Task<CommandResult> ListProjectPolicies(AwsRekognitionListProjectPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStreamProcessors(AwsRekognitionListStreamProcessorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionListStreamProcessorsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsRekognitionListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUsers(AwsRekognitionListUsersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutProjectPolicy(AwsRekognitionPutProjectPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RecognizeCelebrities(AwsRekognitionRecognizeCelebritiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRekognitionRecognizeCelebritiesOptions(), token);
    }

    public async Task<CommandResult> SearchFaces(AwsRekognitionSearchFacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchFacesByImage(AwsRekognitionSearchFacesByImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchUsers(AwsRekognitionSearchUsersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchUsersByImage(AwsRekognitionSearchUsersByImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartCelebrityRecognition(AwsRekognitionStartCelebrityRecognitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartContentModeration(AwsRekognitionStartContentModerationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartFaceDetection(AwsRekognitionStartFaceDetectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartFaceSearch(AwsRekognitionStartFaceSearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartLabelDetection(AwsRekognitionStartLabelDetectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMediaAnalysisJob(AwsRekognitionStartMediaAnalysisJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartPersonTracking(AwsRekognitionStartPersonTrackingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartProjectVersion(AwsRekognitionStartProjectVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSegmentDetection(AwsRekognitionStartSegmentDetectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartStreamProcessor(AwsRekognitionStartStreamProcessorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartTextDetection(AwsRekognitionStartTextDetectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopProjectVersion(AwsRekognitionStopProjectVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopStreamProcessor(AwsRekognitionStopStreamProcessorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsRekognitionTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsRekognitionUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDatasetEntries(AwsRekognitionUpdateDatasetEntriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateStreamProcessor(AwsRekognitionUpdateStreamProcessorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}