using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsMigrationhubConfig
{
    public AwsMigrationhubConfig(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateHomeRegionControl(AwsMigrationhubConfigCreateHomeRegionControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteHomeRegionControl(AwsMigrationhubConfigDeleteHomeRegionControlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeHomeRegionControls(AwsMigrationhubConfigDescribeHomeRegionControlsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubConfigDescribeHomeRegionControlsOptions(), token);
    }

    public async Task<CommandResult> GetHomeRegion(AwsMigrationhubConfigGetHomeRegionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubConfigGetHomeRegionOptions(), token);
    }
}