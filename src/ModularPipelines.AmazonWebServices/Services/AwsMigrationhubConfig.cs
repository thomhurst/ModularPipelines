using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CreateHomeRegionControl(AwsMigrationhubConfigCreateHomeRegionControlOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteHomeRegionControl(AwsMigrationhubConfigDeleteHomeRegionControlOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeHomeRegionControls(AwsMigrationhubConfigDescribeHomeRegionControlsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubConfigDescribeHomeRegionControlsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetHomeRegion(AwsMigrationhubConfigGetHomeRegionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMigrationhubConfigGetHomeRegionOptions(), executionOptions, cancellationToken);
    }
}