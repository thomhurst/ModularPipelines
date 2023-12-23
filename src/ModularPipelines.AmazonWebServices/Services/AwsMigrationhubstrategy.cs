using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> GetApplicationComponentDetails(AwsMigrationhubstrategyGetApplicationComponentDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetApplicationComponentStrategies(AwsMigrationhubstrategyGetApplicationComponentStrategiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAssessment(AwsMigrationhubstrategyGetAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetImportFileTask(AwsMigrationhubstrategyGetImportFileTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLatestAssessmentId(AwsMigrationhubstrategyGetLatestAssessmentIdOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyGetLatestAssessmentIdOptions(), token);
    }

    public async Task<CommandResult> GetPortfolioPreferences(AwsMigrationhubstrategyGetPortfolioPreferencesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyGetPortfolioPreferencesOptions(), token);
    }

    public async Task<CommandResult> GetPortfolioSummary(AwsMigrationhubstrategyGetPortfolioSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyGetPortfolioSummaryOptions(), token);
    }

    public async Task<CommandResult> GetRecommendationReportDetails(AwsMigrationhubstrategyGetRecommendationReportDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServerDetails(AwsMigrationhubstrategyGetServerDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServerStrategies(AwsMigrationhubstrategyGetServerStrategiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAnalyzableServers(AwsMigrationhubstrategyListAnalyzableServersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyListAnalyzableServersOptions(), token);
    }

    public async Task<CommandResult> ListApplicationComponents(AwsMigrationhubstrategyListApplicationComponentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyListApplicationComponentsOptions(), token);
    }

    public async Task<CommandResult> ListCollectors(AwsMigrationhubstrategyListCollectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyListCollectorsOptions(), token);
    }

    public async Task<CommandResult> ListImportFileTask(AwsMigrationhubstrategyListImportFileTaskOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyListImportFileTaskOptions(), token);
    }

    public async Task<CommandResult> ListServers(AwsMigrationhubstrategyListServersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyListServersOptions(), token);
    }

    public async Task<CommandResult> PutPortfolioPreferences(AwsMigrationhubstrategyPutPortfolioPreferencesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyPutPortfolioPreferencesOptions(), token);
    }

    public async Task<CommandResult> StartAssessment(AwsMigrationhubstrategyStartAssessmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyStartAssessmentOptions(), token);
    }

    public async Task<CommandResult> StartImportFileTask(AwsMigrationhubstrategyStartImportFileTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartRecommendationReportGeneration(AwsMigrationhubstrategyStartRecommendationReportGenerationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubstrategyStartRecommendationReportGenerationOptions(), token);
    }

    public async Task<CommandResult> StopAssessment(AwsMigrationhubstrategyStopAssessmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApplicationComponentConfig(AwsMigrationhubstrategyUpdateApplicationComponentConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateServerConfig(AwsMigrationhubstrategyUpdateServerConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}