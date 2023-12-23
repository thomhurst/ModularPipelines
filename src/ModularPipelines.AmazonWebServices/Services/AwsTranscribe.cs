using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsTranscribe
{
    public AwsTranscribe(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateCallAnalyticsCategory(AwsTranscribeCreateCallAnalyticsCategoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLanguageModel(AwsTranscribeCreateLanguageModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMedicalVocabulary(AwsTranscribeCreateMedicalVocabularyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVocabulary(AwsTranscribeCreateVocabularyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVocabularyFilter(AwsTranscribeCreateVocabularyFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCallAnalyticsCategory(AwsTranscribeDeleteCallAnalyticsCategoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCallAnalyticsJob(AwsTranscribeDeleteCallAnalyticsJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLanguageModel(AwsTranscribeDeleteLanguageModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMedicalScribeJob(AwsTranscribeDeleteMedicalScribeJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMedicalTranscriptionJob(AwsTranscribeDeleteMedicalTranscriptionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMedicalVocabulary(AwsTranscribeDeleteMedicalVocabularyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTranscriptionJob(AwsTranscribeDeleteTranscriptionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVocabulary(AwsTranscribeDeleteVocabularyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVocabularyFilter(AwsTranscribeDeleteVocabularyFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLanguageModel(AwsTranscribeDescribeLanguageModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCallAnalyticsCategory(AwsTranscribeGetCallAnalyticsCategoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCallAnalyticsJob(AwsTranscribeGetCallAnalyticsJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMedicalScribeJob(AwsTranscribeGetMedicalScribeJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMedicalTranscriptionJob(AwsTranscribeGetMedicalTranscriptionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMedicalVocabulary(AwsTranscribeGetMedicalVocabularyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTranscriptionJob(AwsTranscribeGetTranscriptionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVocabulary(AwsTranscribeGetVocabularyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVocabularyFilter(AwsTranscribeGetVocabularyFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCallAnalyticsCategories(AwsTranscribeListCallAnalyticsCategoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListCallAnalyticsCategoriesOptions(), token);
    }

    public async Task<CommandResult> ListCallAnalyticsJobs(AwsTranscribeListCallAnalyticsJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListCallAnalyticsJobsOptions(), token);
    }

    public async Task<CommandResult> ListLanguageModels(AwsTranscribeListLanguageModelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListLanguageModelsOptions(), token);
    }

    public async Task<CommandResult> ListMedicalScribeJobs(AwsTranscribeListMedicalScribeJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListMedicalScribeJobsOptions(), token);
    }

    public async Task<CommandResult> ListMedicalTranscriptionJobs(AwsTranscribeListMedicalTranscriptionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListMedicalTranscriptionJobsOptions(), token);
    }

    public async Task<CommandResult> ListMedicalVocabularies(AwsTranscribeListMedicalVocabulariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListMedicalVocabulariesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsTranscribeListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTranscriptionJobs(AwsTranscribeListTranscriptionJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListTranscriptionJobsOptions(), token);
    }

    public async Task<CommandResult> ListVocabularies(AwsTranscribeListVocabulariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListVocabulariesOptions(), token);
    }

    public async Task<CommandResult> ListVocabularyFilters(AwsTranscribeListVocabularyFiltersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListVocabularyFiltersOptions(), token);
    }

    public async Task<CommandResult> StartCallAnalyticsJob(AwsTranscribeStartCallAnalyticsJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMedicalScribeJob(AwsTranscribeStartMedicalScribeJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMedicalTranscriptionJob(AwsTranscribeStartMedicalTranscriptionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartTranscriptionJob(AwsTranscribeStartTranscriptionJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsTranscribeTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsTranscribeUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCallAnalyticsCategory(AwsTranscribeUpdateCallAnalyticsCategoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMedicalVocabulary(AwsTranscribeUpdateMedicalVocabularyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVocabulary(AwsTranscribeUpdateVocabularyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVocabularyFilter(AwsTranscribeUpdateVocabularyFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}