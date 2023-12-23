using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsDiscovery
{
    public AwsDiscovery(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateConfigurationItemsToApplication(AwsDiscoveryAssociateConfigurationItemsToApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteAgents(AwsDiscoveryBatchDeleteAgentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeleteImportData(AwsDiscoveryBatchDeleteImportDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateApplication(AwsDiscoveryCreateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTags(AwsDiscoveryCreateTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApplications(AwsDiscoveryDeleteApplicationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTags(AwsDiscoveryDeleteTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAgents(AwsDiscoveryDescribeAgentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDiscoveryDescribeAgentsOptions(), token);
    }

    public async Task<CommandResult> DescribeBatchDeleteConfigurationTask(AwsDiscoveryDescribeBatchDeleteConfigurationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConfigurations(AwsDiscoveryDescribeConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeContinuousExports(AwsDiscoveryDescribeContinuousExportsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDiscoveryDescribeContinuousExportsOptions(), token);
    }

    public async Task<CommandResult> DescribeExportTasks(AwsDiscoveryDescribeExportTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDiscoveryDescribeExportTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeImportTasks(AwsDiscoveryDescribeImportTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDiscoveryDescribeImportTasksOptions(), token);
    }

    public async Task<CommandResult> DescribeTags(AwsDiscoveryDescribeTagsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDiscoveryDescribeTagsOptions(), token);
    }

    public async Task<CommandResult> DisassociateConfigurationItemsFromApplication(AwsDiscoveryDisassociateConfigurationItemsFromApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDiscoverySummary(AwsDiscoveryGetDiscoverySummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDiscoveryGetDiscoverySummaryOptions(), token);
    }

    public async Task<CommandResult> ListConfigurations(AwsDiscoveryListConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListServerNeighbors(AwsDiscoveryListServerNeighborsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartBatchDeleteConfigurationTask(AwsDiscoveryStartBatchDeleteConfigurationTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartContinuousExport(AwsDiscoveryStartContinuousExportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDiscoveryStartContinuousExportOptions(), token);
    }

    public async Task<CommandResult> StartDataCollectionByAgentIds(AwsDiscoveryStartDataCollectionByAgentIdsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartExportTask(AwsDiscoveryStartExportTaskOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDiscoveryStartExportTaskOptions(), token);
    }

    public async Task<CommandResult> StartImportTask(AwsDiscoveryStartImportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopContinuousExport(AwsDiscoveryStopContinuousExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopDataCollectionByAgentIds(AwsDiscoveryStopDataCollectionByAgentIdsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApplication(AwsDiscoveryUpdateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}