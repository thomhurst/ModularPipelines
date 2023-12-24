using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsBedrock
{
    public AwsBedrock(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateModelCustomizationJob(AwsBedrockCreateModelCustomizationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProvisionedModelThroughput(AwsBedrockCreateProvisionedModelThroughputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomModel(AwsBedrockDeleteCustomModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModelInvocationLoggingConfiguration(AwsBedrockDeleteModelInvocationLoggingConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBedrockDeleteModelInvocationLoggingConfigurationOptions(), token);
    }

    public async Task<CommandResult> DeleteProvisionedModelThroughput(AwsBedrockDeleteProvisionedModelThroughputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCustomModel(AwsBedrockGetCustomModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFoundationModel(AwsBedrockGetFoundationModelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetModelCustomizationJob(AwsBedrockGetModelCustomizationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetModelInvocationLoggingConfiguration(AwsBedrockGetModelInvocationLoggingConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBedrockGetModelInvocationLoggingConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetProvisionedModelThroughput(AwsBedrockGetProvisionedModelThroughputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomModels(AwsBedrockListCustomModelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBedrockListCustomModelsOptions(), token);
    }

    public async Task<CommandResult> ListFoundationModels(AwsBedrockListFoundationModelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBedrockListFoundationModelsOptions(), token);
    }

    public async Task<CommandResult> ListModelCustomizationJobs(AwsBedrockListModelCustomizationJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBedrockListModelCustomizationJobsOptions(), token);
    }

    public async Task<CommandResult> ListProvisionedModelThroughputs(AwsBedrockListProvisionedModelThroughputsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsBedrockListProvisionedModelThroughputsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsBedrockListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutModelInvocationLoggingConfiguration(AwsBedrockPutModelInvocationLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopModelCustomizationJob(AwsBedrockStopModelCustomizationJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsBedrockTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsBedrockUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProvisionedModelThroughput(AwsBedrockUpdateProvisionedModelThroughputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}