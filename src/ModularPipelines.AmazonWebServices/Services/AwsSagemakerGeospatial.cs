using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSagemakerGeospatial
{
    public AwsSagemakerGeospatial(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DeleteEarthObservationJob(AwsSagemakerGeospatialDeleteEarthObservationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVectorEnrichmentJob(AwsSagemakerGeospatialDeleteVectorEnrichmentJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportEarthObservationJob(AwsSagemakerGeospatialExportEarthObservationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExportVectorEnrichmentJob(AwsSagemakerGeospatialExportVectorEnrichmentJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEarthObservationJob(AwsSagemakerGeospatialGetEarthObservationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRasterDataCollection(AwsSagemakerGeospatialGetRasterDataCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTile(AwsSagemakerGeospatialGetTileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVectorEnrichmentJob(AwsSagemakerGeospatialGetVectorEnrichmentJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEarthObservationJobs(AwsSagemakerGeospatialListEarthObservationJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerGeospatialListEarthObservationJobsOptions(), token);
    }

    public async Task<CommandResult> ListRasterDataCollections(AwsSagemakerGeospatialListRasterDataCollectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerGeospatialListRasterDataCollectionsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsSagemakerGeospatialListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVectorEnrichmentJobs(AwsSagemakerGeospatialListVectorEnrichmentJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSagemakerGeospatialListVectorEnrichmentJobsOptions(), token);
    }

    public async Task<CommandResult> SearchRasterDataCollection(AwsSagemakerGeospatialSearchRasterDataCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartEarthObservationJob(AwsSagemakerGeospatialStartEarthObservationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartVectorEnrichmentJob(AwsSagemakerGeospatialStartVectorEnrichmentJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopEarthObservationJob(AwsSagemakerGeospatialStopEarthObservationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopVectorEnrichmentJob(AwsSagemakerGeospatialStopVectorEnrichmentJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsSagemakerGeospatialTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsSagemakerGeospatialUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}