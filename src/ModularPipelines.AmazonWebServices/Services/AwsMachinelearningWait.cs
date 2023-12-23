using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning")]
public class AwsMachinelearningWait
{
    public AwsMachinelearningWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchPredictionAvailable(AwsMachinelearningWaitBatchPredictionAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningWaitBatchPredictionAvailableOptions(), token);
    }

    public async Task<CommandResult> DataSourceAvailable(AwsMachinelearningWaitDataSourceAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningWaitDataSourceAvailableOptions(), token);
    }

    public async Task<CommandResult> EvaluationAvailable(AwsMachinelearningWaitEvaluationAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningWaitEvaluationAvailableOptions(), token);
    }

    public async Task<CommandResult> MlModelAvailable(AwsMachinelearningWaitMlModelAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMachinelearningWaitMlModelAvailableOptions(), token);
    }
}