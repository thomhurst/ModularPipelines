using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("machinelearning")]
public class AwsMachinelearningWait
{
    public AwsMachinelearningWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchPredictionAvailable(AwsMachinelearningWaitBatchPredictionAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningWaitBatchPredictionAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DataSourceAvailable(AwsMachinelearningWaitDataSourceAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningWaitDataSourceAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EvaluationAvailable(AwsMachinelearningWaitEvaluationAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningWaitEvaluationAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> MlModelAvailable(AwsMachinelearningWaitMlModelAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningWaitMlModelAvailableOptions(), executionOptions, cancellationToken);
    }
}