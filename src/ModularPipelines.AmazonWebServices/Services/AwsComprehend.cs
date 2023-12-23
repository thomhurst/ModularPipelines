using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsComprehend
{
    public AwsComprehend(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchDetectDominantLanguage(AwsComprehendBatchDetectDominantLanguageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDetectEntities(AwsComprehendBatchDetectEntitiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDetectKeyPhrases(AwsComprehendBatchDetectKeyPhrasesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDetectSentiment(AwsComprehendBatchDetectSentimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDetectSyntax(AwsComprehendBatchDetectSyntaxOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDetectTargetedSentiment(AwsComprehendBatchDetectTargetedSentimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ClassifyDocument(AwsComprehendClassifyDocumentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainsPiiEntities(AwsComprehendContainsPiiEntitiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataset(AwsComprehendCreateDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDocumentClassifier(AwsComprehendCreateDocumentClassifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEndpoint(AwsComprehendCreateEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEntityRecognizer(AwsComprehendCreateEntityRecognizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFlywheel(AwsComprehendCreateFlywheelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDocumentClassifier(AwsComprehendDeleteDocumentClassifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEndpoint(AwsComprehendDeleteEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEntityRecognizer(AwsComprehendDeleteEntityRecognizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFlywheel(AwsComprehendDeleteFlywheelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsComprehendDeleteResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDataset(AwsComprehendDescribeDatasetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDocumentClassificationJob(AwsComprehendDescribeDocumentClassificationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDocumentClassifier(AwsComprehendDescribeDocumentClassifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDominantLanguageDetectionJob(AwsComprehendDescribeDominantLanguageDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEndpoint(AwsComprehendDescribeEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEntitiesDetectionJob(AwsComprehendDescribeEntitiesDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEntityRecognizer(AwsComprehendDescribeEntityRecognizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEventsDetectionJob(AwsComprehendDescribeEventsDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFlywheel(AwsComprehendDescribeFlywheelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFlywheelIteration(AwsComprehendDescribeFlywheelIterationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeKeyPhrasesDetectionJob(AwsComprehendDescribeKeyPhrasesDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePiiEntitiesDetectionJob(AwsComprehendDescribePiiEntitiesDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeResourcePolicy(AwsComprehendDescribeResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSentimentDetectionJob(AwsComprehendDescribeSentimentDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTargetedSentimentDetectionJob(AwsComprehendDescribeTargetedSentimentDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTopicsDetectionJob(AwsComprehendDescribeTopicsDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectDominantLanguage(AwsComprehendDetectDominantLanguageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectEntities(AwsComprehendDetectEntitiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendDetectEntitiesOptions(), token);
    }

    public async Task<CommandResult> DetectKeyPhrases(AwsComprehendDetectKeyPhrasesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectPiiEntities(AwsComprehendDetectPiiEntitiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectSentiment(AwsComprehendDetectSentimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectSyntax(AwsComprehendDetectSyntaxOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectTargetedSentiment(AwsComprehendDetectTargetedSentimentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetectToxicContent(AwsComprehendDetectToxicContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportModel(AwsComprehendImportModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDatasets(AwsComprehendListDatasetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListDatasetsOptions(), token);
    }

    public async Task<CommandResult> ListDocumentClassificationJobs(AwsComprehendListDocumentClassificationJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListDocumentClassificationJobsOptions(), token);
    }

    public async Task<CommandResult> ListDocumentClassifierSummaries(AwsComprehendListDocumentClassifierSummariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListDocumentClassifierSummariesOptions(), token);
    }

    public async Task<CommandResult> ListDocumentClassifiers(AwsComprehendListDocumentClassifiersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListDocumentClassifiersOptions(), token);
    }

    public async Task<CommandResult> ListDominantLanguageDetectionJobs(AwsComprehendListDominantLanguageDetectionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListDominantLanguageDetectionJobsOptions(), token);
    }

    public async Task<CommandResult> ListEndpoints(AwsComprehendListEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListEndpointsOptions(), token);
    }

    public async Task<CommandResult> ListEntitiesDetectionJobs(AwsComprehendListEntitiesDetectionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListEntitiesDetectionJobsOptions(), token);
    }

    public async Task<CommandResult> ListEntityRecognizerSummaries(AwsComprehendListEntityRecognizerSummariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListEntityRecognizerSummariesOptions(), token);
    }

    public async Task<CommandResult> ListEntityRecognizers(AwsComprehendListEntityRecognizersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListEntityRecognizersOptions(), token);
    }

    public async Task<CommandResult> ListEventsDetectionJobs(AwsComprehendListEventsDetectionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListEventsDetectionJobsOptions(), token);
    }

    public async Task<CommandResult> ListFlywheelIterationHistory(AwsComprehendListFlywheelIterationHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFlywheels(AwsComprehendListFlywheelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListFlywheelsOptions(), token);
    }

    public async Task<CommandResult> ListKeyPhrasesDetectionJobs(AwsComprehendListKeyPhrasesDetectionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListKeyPhrasesDetectionJobsOptions(), token);
    }

    public async Task<CommandResult> ListPiiEntitiesDetectionJobs(AwsComprehendListPiiEntitiesDetectionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListPiiEntitiesDetectionJobsOptions(), token);
    }

    public async Task<CommandResult> ListSentimentDetectionJobs(AwsComprehendListSentimentDetectionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListSentimentDetectionJobsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsComprehendListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTargetedSentimentDetectionJobs(AwsComprehendListTargetedSentimentDetectionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListTargetedSentimentDetectionJobsOptions(), token);
    }

    public async Task<CommandResult> ListTopicsDetectionJobs(AwsComprehendListTopicsDetectionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsComprehendListTopicsDetectionJobsOptions(), token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsComprehendPutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartDocumentClassificationJob(AwsComprehendStartDocumentClassificationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartDominantLanguageDetectionJob(AwsComprehendStartDominantLanguageDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartEntitiesDetectionJob(AwsComprehendStartEntitiesDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartEventsDetectionJob(AwsComprehendStartEventsDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartFlywheelIteration(AwsComprehendStartFlywheelIterationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartKeyPhrasesDetectionJob(AwsComprehendStartKeyPhrasesDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartPiiEntitiesDetectionJob(AwsComprehendStartPiiEntitiesDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSentimentDetectionJob(AwsComprehendStartSentimentDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartTargetedSentimentDetectionJob(AwsComprehendStartTargetedSentimentDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartTopicsDetectionJob(AwsComprehendStartTopicsDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopDominantLanguageDetectionJob(AwsComprehendStopDominantLanguageDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopEntitiesDetectionJob(AwsComprehendStopEntitiesDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopEventsDetectionJob(AwsComprehendStopEventsDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopKeyPhrasesDetectionJob(AwsComprehendStopKeyPhrasesDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopPiiEntitiesDetectionJob(AwsComprehendStopPiiEntitiesDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopSentimentDetectionJob(AwsComprehendStopSentimentDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopTargetedSentimentDetectionJob(AwsComprehendStopTargetedSentimentDetectionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopTrainingDocumentClassifier(AwsComprehendStopTrainingDocumentClassifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopTrainingEntityRecognizer(AwsComprehendStopTrainingEntityRecognizerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsComprehendTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsComprehendUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEndpoint(AwsComprehendUpdateEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFlywheel(AwsComprehendUpdateFlywheelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}