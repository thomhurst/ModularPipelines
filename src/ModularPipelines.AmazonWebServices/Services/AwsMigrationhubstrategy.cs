using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsMigrationhubstrategy
{
    public AwsMigrationhubstrategy(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> GetApplicationComponentDetails(AwsMigrationhubstrategyGetApplicationComponentDetailsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetApplicationComponentStrategies(AwsMigrationhubstrategyGetApplicationComponentStrategiesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAssessment(AwsMigrationhubstrategyGetAssessmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetImportFileTask(AwsMigrationhubstrategyGetImportFileTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetLatestAssessmentId(AwsMigrationhubstrategyGetLatestAssessmentIdOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyGetLatestAssessmentIdOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPortfolioPreferences(AwsMigrationhubstrategyGetPortfolioPreferencesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyGetPortfolioPreferencesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPortfolioSummary(AwsMigrationhubstrategyGetPortfolioSummaryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyGetPortfolioSummaryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetRecommendationReportDetails(AwsMigrationhubstrategyGetRecommendationReportDetailsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetServerDetails(AwsMigrationhubstrategyGetServerDetailsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetServerStrategies(AwsMigrationhubstrategyGetServerStrategiesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAnalyzableServers(AwsMigrationhubstrategyListAnalyzableServersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyListAnalyzableServersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListApplicationComponents(AwsMigrationhubstrategyListApplicationComponentsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyListApplicationComponentsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListCollectors(AwsMigrationhubstrategyListCollectorsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyListCollectorsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListImportFileTask(AwsMigrationhubstrategyListImportFileTaskOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyListImportFileTaskOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListServers(AwsMigrationhubstrategyListServersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyListServersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutPortfolioPreferences(AwsMigrationhubstrategyPutPortfolioPreferencesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyPutPortfolioPreferencesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartAssessment(AwsMigrationhubstrategyStartAssessmentOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyStartAssessmentOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartImportFileTask(AwsMigrationhubstrategyStartImportFileTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartRecommendationReportGeneration(AwsMigrationhubstrategyStartRecommendationReportGenerationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyStartRecommendationReportGenerationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopAssessment(AwsMigrationhubstrategyStopAssessmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateApplicationComponentConfig(AwsMigrationhubstrategyUpdateApplicationComponentConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateServerConfig(AwsMigrationhubstrategyUpdateServerConfigOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}