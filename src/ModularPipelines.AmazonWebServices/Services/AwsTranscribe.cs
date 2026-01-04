using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CreateCallAnalyticsCategory(AwsTranscribeCreateCallAnalyticsCategoryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateLanguageModel(AwsTranscribeCreateLanguageModelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMedicalVocabulary(AwsTranscribeCreateMedicalVocabularyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVocabulary(AwsTranscribeCreateVocabularyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVocabularyFilter(AwsTranscribeCreateVocabularyFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteCallAnalyticsCategory(AwsTranscribeDeleteCallAnalyticsCategoryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteCallAnalyticsJob(AwsTranscribeDeleteCallAnalyticsJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteLanguageModel(AwsTranscribeDeleteLanguageModelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMedicalScribeJob(AwsTranscribeDeleteMedicalScribeJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMedicalTranscriptionJob(AwsTranscribeDeleteMedicalTranscriptionJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMedicalVocabulary(AwsTranscribeDeleteMedicalVocabularyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTranscriptionJob(AwsTranscribeDeleteTranscriptionJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVocabulary(AwsTranscribeDeleteVocabularyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVocabularyFilter(AwsTranscribeDeleteVocabularyFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeLanguageModel(AwsTranscribeDescribeLanguageModelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetCallAnalyticsCategory(AwsTranscribeGetCallAnalyticsCategoryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetCallAnalyticsJob(AwsTranscribeGetCallAnalyticsJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMedicalScribeJob(AwsTranscribeGetMedicalScribeJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMedicalTranscriptionJob(AwsTranscribeGetMedicalTranscriptionJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMedicalVocabulary(AwsTranscribeGetMedicalVocabularyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTranscriptionJob(AwsTranscribeGetTranscriptionJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVocabulary(AwsTranscribeGetVocabularyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVocabularyFilter(AwsTranscribeGetVocabularyFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListCallAnalyticsCategories(AwsTranscribeListCallAnalyticsCategoriesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListCallAnalyticsCategoriesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListCallAnalyticsJobs(AwsTranscribeListCallAnalyticsJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListCallAnalyticsJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListLanguageModels(AwsTranscribeListLanguageModelsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListLanguageModelsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMedicalScribeJobs(AwsTranscribeListMedicalScribeJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListMedicalScribeJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMedicalTranscriptionJobs(AwsTranscribeListMedicalTranscriptionJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListMedicalTranscriptionJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMedicalVocabularies(AwsTranscribeListMedicalVocabulariesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListMedicalVocabulariesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsTranscribeListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTranscriptionJobs(AwsTranscribeListTranscriptionJobsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListTranscriptionJobsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListVocabularies(AwsTranscribeListVocabulariesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListVocabulariesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListVocabularyFilters(AwsTranscribeListVocabularyFiltersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTranscribeListVocabularyFiltersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartCallAnalyticsJob(AwsTranscribeStartCallAnalyticsJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartMedicalScribeJob(AwsTranscribeStartMedicalScribeJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartMedicalTranscriptionJob(AwsTranscribeStartMedicalTranscriptionJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartTranscriptionJob(AwsTranscribeStartTranscriptionJobOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsTranscribeTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsTranscribeUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateCallAnalyticsCategory(AwsTranscribeUpdateCallAnalyticsCategoryOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateMedicalVocabulary(AwsTranscribeUpdateMedicalVocabularyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateVocabulary(AwsTranscribeUpdateVocabularyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateVocabularyFilter(AwsTranscribeUpdateVocabularyFilterOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}