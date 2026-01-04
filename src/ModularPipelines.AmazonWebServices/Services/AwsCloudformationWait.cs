using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloudformation")]
public class AwsCloudformationWait
{
    public AwsCloudformationWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ChangeSetCreateComplete(AwsCloudformationWaitChangeSetCreateCompleteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StackCreateComplete(AwsCloudformationWaitStackCreateCompleteOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackCreateCompleteOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StackDeleteComplete(AwsCloudformationWaitStackDeleteCompleteOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackDeleteCompleteOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StackExists(AwsCloudformationWaitStackExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StackImportComplete(AwsCloudformationWaitStackImportCompleteOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackImportCompleteOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StackRollbackComplete(AwsCloudformationWaitStackRollbackCompleteOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackRollbackCompleteOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StackUpdateComplete(AwsCloudformationWaitStackUpdateCompleteOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCloudformationWaitStackUpdateCompleteOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TypeRegistrationComplete(AwsCloudformationWaitTypeRegistrationCompleteOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}