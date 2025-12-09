using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("sagemaker")]
public class AwsSagemakerWait
{
    public AwsSagemakerWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> EndpointDeleted(AwsSagemakerWaitEndpointDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EndpointInService(AwsSagemakerWaitEndpointInServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImageCreated(AwsSagemakerWaitImageCreatedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImageDeleted(AwsSagemakerWaitImageDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImageUpdated(AwsSagemakerWaitImageUpdatedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImageVersionCreated(AwsSagemakerWaitImageVersionCreatedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImageVersionDeleted(AwsSagemakerWaitImageVersionDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NotebookInstanceDeleted(AwsSagemakerWaitNotebookInstanceDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NotebookInstanceInService(AwsSagemakerWaitNotebookInstanceInServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NotebookInstanceStopped(AwsSagemakerWaitNotebookInstanceStoppedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ProcessingJobCompletedOrStopped(AwsSagemakerWaitProcessingJobCompletedOrStoppedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TrainingJobCompletedOrStopped(AwsSagemakerWaitTrainingJobCompletedOrStoppedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TransformJobCompletedOrStopped(AwsSagemakerWaitTransformJobCompletedOrStoppedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}